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

export interface TranslationFile {
  id: string;
  translationId: string;
  fileName: string;
  contentType: string;
  size: number;
  uploadedAt: string;
}

export interface Translation {
  id: string;
  entityId: string;
  langId: number;
  langValue: string;
  title: string;
  content: string | null;
  modifiedAt: string;
  files: TranslationFile[];
}

export interface Entity {
  id: string;
  typeId: number;
  typeName: string;
  parentId: string | null;
  createdAt: string;
  modifiedAt: string;
  translations: Translation[];
}

export interface TranslationContent {
  langId: number;
  title: string;
  content?: string | null;
}

export interface CreateEntityRequest {
  typeId: number;
  parentId: string | null;
  translations: TranslationContent[];
}

export interface CreateTranslationRequest extends TranslationContent {
  entityId: string;
}

export interface UpdateUserRequest {
  email: string;
  firstName: string;
  lastName: string;
}

export interface ChangePasswordRequest {
  currentPassword: string;
  newPassword: string;
}

export interface DictionaryEntry {
  id: string;
  entityId: string;
  langId: number;
  term: string;
  definition: string;
}

export interface EntityContent {
  id: string;
  typeId: number;
  order: number;
  isCorrect: boolean;
  langId: number;
  translationId: string | null;
  title: string;
  content: string | null;
  hasTranslation: boolean;
  files: TranslationFile[];
  children: EntityContent[];
}
