export type Role = 'User' | 'Moderator' | 'Admin';

export interface User {
  id: number;
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
  createdAt: string;
  lastLoginAt: string | null;
  groups: GroupSummary[];
}

export interface GroupSummary {
  id: number;
  name: string;
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
  order: number;
  isBlocked: boolean;
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
  order: number;
  isCorrect: boolean;
  isBlocked: boolean;
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

export interface UpdateEntityRequest {
  order: number;
  isCorrect: boolean;
  isBlocked: boolean;
}

export interface PagedResult<T> {
  items: T[];
  page: number;
  pageSize: number;
  totalCount: number;
  totalPages: number;
}

export interface PagingParams {
  page: number;
  pageSize: number;
  search?: string;
}

export interface Group {
  id: number;
  name: string;
  description: string | null;
  createdAt: string;
  usersCount: number;
}

export interface UserAdminRequest {
  email: string;
  firstName: string;
  lastName: string;
  role: Role;
  groupIds: number[];
  password?: string;
}

export interface GroupRequest {
  name: string;
  description: string | null;
}

export type PendingChangeType = 'Added' | 'Modified' | 'Removed';

export interface PendingEntityChange {
  entityId: string;
  typeId: number;
  title: string;
  changeType: PendingChangeType;
  modifiedAt: string;
}

export interface PendingChanges {
  languagesChanged: boolean;
  hasChanges: boolean;
  entities: PendingEntityChange[];
}

export interface SynchronizationResult {
  publishedAt: string;
  languages: number;
  entities: number;
  translations: number;
  files: number;
  dictionaryEntries: number;
  removedFiles: number;
}

export interface EntitySearchResult {
  entityId: string;
  typeId: number;
  title: string;
  pathIds: string[];
  path: string[];
}

export interface EntityType {
  id: number;
  name: string;
  canBeRoot: boolean;
  allowedChildTypeIds: number[];
}
