import apiClient from '../apiClient';
import { DictionaryEntry } from '../types';

type DictionaryEntryRequest = Omit<DictionaryEntry, 'id'>;
type DictionaryEntryUpdate = Pick<DictionaryEntry, 'term' | 'definition'>;

export const getDictionary = (entityId: string, langId: number) =>
  apiClient.get<DictionaryEntry[]>(`/EntityDictionary/entity/${entityId}`, { params: { langId } }).then((r) => r.data);

export const createDictionaryEntry = (entry: DictionaryEntryRequest) =>
  apiClient.post<DictionaryEntry>('/EntityDictionary', entry).then((r) => r.data);

export const updateDictionaryEntry = (id: string, entry: DictionaryEntryUpdate) =>
  apiClient.put<DictionaryEntry>(`/EntityDictionary/${id}`, entry).then((r) => r.data);

export const deleteDictionaryEntry = (id: string) => apiClient.delete(`/EntityDictionary/${id}`);
