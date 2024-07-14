import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { User } from '../network/types';

export interface Tokens {
  accessToken: string;
  refreshToken: string | null;
  expiresAt: number;
}

interface StoreState {
  accessToken: string | null;
  refreshToken: string | null;
  expiresAt: number | null;
  user: User | null;
  learningLangId: number | null;
  nativeLangId: number | null;
  moderatorLangId: number | null;
  setTokens: (tokens: Tokens) => void;
  setUser: (user: User) => void;
  clearSession: () => void;
  setLanguages: (learningLangId: number, nativeLangId: number) => void;
  setModeratorLangId: (langId: number) => void;
}

const STORE_VERSION = 1;

const useStore = create<StoreState>()(
  persist(
    (set) => ({
      accessToken: null,
      refreshToken: null,
      expiresAt: null,
      user: null,
      learningLangId: null,
      nativeLangId: null,
      moderatorLangId: null,
      setTokens: (tokens) => set(tokens),
      setUser: (user) => set({ user }),
      clearSession: () => set({ accessToken: null, refreshToken: null, expiresAt: null, user: null }),
      setLanguages: (learningLangId, nativeLangId) => set({ learningLangId, nativeLangId }),
      setModeratorLangId: (moderatorLangId) => set({ moderatorLangId }),
    }),
    // Version 1 replaced the API session token with Cognito tokens.
    { name: 'medicase', version: STORE_VERSION }
  )
);

/** The user is signed in when both the tokens and the profile from the API are known. */
export const selectIsSignedIn = (state: StoreState) => Boolean(state.accessToken && state.user);

export default useStore;
