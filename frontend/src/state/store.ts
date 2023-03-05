import { create } from 'zustand';
import { persist } from 'zustand/middleware';
import { Session, User } from '../network/types';

interface StoreState {
  token: string | null;
  user: User | null;
  learningLangId: number | null;
  nativeLangId: number | null;
  setSession: (session: Session) => void;
  setUser: (user: User) => void;
  clearSession: () => void;
  setLanguages: (learningLangId: number, nativeLangId: number) => void;
}

const useStore = create<StoreState>()(
  persist(
    (set) => ({
      token: null,
      user: null,
      learningLangId: null,
      nativeLangId: null,
      setSession: (session) => set({ token: session.token, user: session.user }),
      setUser: (user) => set({ user }),
      clearSession: () => set({ token: null, user: null }),
      setLanguages: (learningLangId, nativeLangId) => set({ learningLangId, nativeLangId }),
    }),
    { name: 'premedical' }
  )
);

export default useStore;
