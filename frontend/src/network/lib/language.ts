import apiClient from '../apiClient';
import { Language } from '../types';

type LanguageRequest = Omit<Language, 'id'>;

export const getLanguages = () => apiClient.get<Language[]>('/EntityLanguages').then((r) => r.data);

export const createLanguage = (language: LanguageRequest) =>
  apiClient.post<Language>('/EntityLanguages', language).then((r) => r.data);

export const updateLanguage = (id: number, language: LanguageRequest) =>
  apiClient.put<Language>(`/EntityLanguages/${id}`, language).then((r) => r.data);

export const deleteLanguage = (id: number) => apiClient.delete(`/EntityLanguages/${id}`);
