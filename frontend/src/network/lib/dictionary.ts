import apiClient from '../apiClient';
import { DictionaryEntry } from '../types';

export const getDictionary = (entityId: string, langId: number) =>
  apiClient.get<DictionaryEntry[]>(`/EntityDictionary/entity/${entityId}`, { params: { langId } }).then((r) => r.data);
