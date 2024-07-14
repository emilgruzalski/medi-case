// Minimal client of the Amazon Cognito user pools API used by the single page app.
// Only unauthenticated operations and operations authorized with the user's access token are called.

const REGION = import.meta.env.VITE_COGNITO_REGION;
const CLIENT_ID = import.meta.env.VITE_COGNITO_CLIENT_ID;
// A local emulator (cognito-local) can be used instead of AWS during development.
const ENDPOINT = import.meta.env.VITE_COGNITO_ENDPOINT || `https://cognito-idp.${REGION}.amazonaws.com/`;

export class CognitoError extends Error {
  code: string;

  constructor(code: string, message: string) {
    super(message);
    this.name = 'CognitoError';
    this.code = code;
  }
}

export interface AuthenticationResult {
  AccessToken: string;
  IdToken: string;
  RefreshToken?: string;
  ExpiresIn?: number;
}

export interface InitiateAuthResponse {
  AuthenticationResult?: AuthenticationResult;
  ChallengeName?: string;
  Session?: string;
}

async function call<T>(action: string, body: object): Promise<T> {
  const response = await fetch(ENDPOINT, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/x-amz-json-1.1',
      'X-Amz-Target': `AWSCognitoIdentityProviderService.${action}`,
    },
    body: JSON.stringify(body),
  });
  const data = await response.json().catch(() => ({}));
  if (!response.ok) {
    const code = String(data.__type ?? 'UnknownError').split('#').pop() as string;
    throw new CognitoError(code, data.message ?? 'Cognito request failed');
  }
  return data as T;
}

export const initiatePasswordAuth = (email: string, password: string) =>
  call<InitiateAuthResponse>('InitiateAuth', {
    AuthFlow: 'USER_PASSWORD_AUTH',
    ClientId: CLIENT_ID,
    AuthParameters: { USERNAME: email, PASSWORD: password },
  });

export const initiateRefreshAuth = (refreshToken: string) =>
  call<InitiateAuthResponse>('InitiateAuth', {
    AuthFlow: 'REFRESH_TOKEN_AUTH',
    ClientId: CLIENT_ID,
    AuthParameters: { REFRESH_TOKEN: refreshToken },
  });

export const signUp = (email: string, password: string, firstName: string, lastName: string) =>
  call('SignUp', {
    ClientId: CLIENT_ID,
    Username: email,
    Password: password,
    UserAttributes: [
      { Name: 'email', Value: email },
      { Name: 'given_name', Value: firstName },
      { Name: 'family_name', Value: lastName },
    ],
  });

export const confirmSignUp = (email: string, code: string) =>
  call('ConfirmSignUp', { ClientId: CLIENT_ID, Username: email, ConfirmationCode: code });

export const changePassword = (accessToken: string, previousPassword: string, proposedPassword: string) =>
  call('ChangePassword', { AccessToken: accessToken, PreviousPassword: previousPassword, ProposedPassword: proposedPassword });

export const revokeToken = (refreshToken: string) => call('RevokeToken', { ClientId: CLIENT_ID, Token: refreshToken });
