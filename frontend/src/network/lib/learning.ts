import apiClient from '../apiClient';
import { DictionaryEntry, EntityContent, Language, TreeNode } from '../types';

// Learners read only the published content.

export const getLearningLanguages = () => apiClient.get<Language[]>('/Learning/languages').then((r) => r.data);

export const getLearningTree = (langId: number) =>
  apiClient.get<TreeNode[]>('/Learning/tree', { params: { langId } }).then((r) => r.data);

export const getLearningEntity = (id: string, langId: number) =>
  apiClient.get<EntityContent>(`/Learning/entity/${id}`, { params: { langId } }).then((r) => r.data);

export const getLearningDictionary = (entityId: string, langId: number) =>
  apiClient
    .get<DictionaryEntry[]>(`/Learning/entity/${entityId}/dictionary`, { params: { langId } })
    .then((r) => r.data);

export const getLearningFileObjectUrl = (fileId: string) =>
  apiClient.get<Blob>(`/Learning/files/${fileId}`, { responseType: 'blob' }).then((r) => URL.createObjectURL(r.data));
