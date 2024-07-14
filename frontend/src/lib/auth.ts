import useStore, { Tokens } from '../state/store';
import { AuthenticationResult, CognitoError, initiatePasswordAuth, initiateRefreshAuth, revokeToken } from '../network/cognito';
import { getCurrentUser } from '../network/lib/user';

// Tokens are refreshed shortly before they expire.
const REFRESH_MARGIN_MS = 60_000;
const MS_IN_SECOND = 1000;

let refreshing: Promise<string | null> | null = null;

function tokenExpiry(token: string): number {
  const payload = token.split('.')[1].replace(/-/g, '+').replace(/_/g, '/');
  return JSON.parse(atob(payload)).exp * MS_IN_SECOND;
}

function toTokens(result: AuthenticationResult, previousRefreshToken: string | null = null): Tokens {
  return {
    accessToken: result.AccessToken,
    refreshToken: result.RefreshToken ?? previousRefreshToken,
    expiresAt: tokenExpiry(result.AccessToken),
  };
}

export async function signIn(email: string, password: string) {
  const response = await initiatePasswordAuth(email.trim().toLowerCase(), password);
  if (!response.AuthenticationResult) {
    throw new CognitoError(response.ChallengeName ?? 'UnknownError', 'Additional sign-in step is required');
  }

  const { setTokens, setUser, clearSession } = useStore.getState();
  setTokens(toTokens(response.AuthenticationResult));
  try {
    setUser(await getCurrentUser());
  } catch (error) {
    clearSession();
    throw error;
  }
}

export async function signOut() {
  const { refreshToken, clearSession } = useStore.getState();
  try {
    if (refreshToken) {
      await revokeToken(refreshToken);
    }
  } finally {
    clearSession();
  }
}

async function refresh(refreshToken: string) {
  const { setTokens, clearSession } = useStore.getState();
  try {
    const response = await initiateRefreshAuth(refreshToken);
    setTokens(toTokens(response.AuthenticationResult as AuthenticationResult, refreshToken));
    return useStore.getState().accessToken;
  } catch {
    clearSession();
    return null;
  }
}

/** Returns an access token that is valid for at least a minute, refreshing it when needed. */
export async function getValidAccessToken(): Promise<string | null> {
  const { accessToken, refreshToken, expiresAt } = useStore.getState();
  if (!accessToken || !expiresAt || expiresAt - Date.now() > REFRESH_MARGIN_MS || !refreshToken) {
    return accessToken;
  }
  refreshing ??= refresh(refreshToken).finally(() => {
    refreshing = null;
  });
  return refreshing;
}
