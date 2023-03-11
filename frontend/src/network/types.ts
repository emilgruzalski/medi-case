export type Role = 'User' | 'Moderator' | 'Admin';

export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
  createdAt: string;
  lastLoginAt: string | null;
}

export interface Session {
  token: string;
  expiresAt: string;
  user: User;
}

export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface Language {
  id: number;
  langValue: string;
  name: string;
}

export interface TreeNode {
  id: string;
  typeId: number;
  title: string;
  hasTranslation: boolean;
  children: TreeNode[];
}
