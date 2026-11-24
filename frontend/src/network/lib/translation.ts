import apiClient from '../apiClient';
import { CreateTranslationRequest, Translation } from '../types';

export const createTranslation = (translation: CreateTranslationRequest) =>
  apiClient.post<Translation>('/EntityTranslations', translation).then((r) => r.data);

export const updateTranslation = (id: string, translation: { title: string; content: string | null }) =>
  apiClient.put<Translation>(`/EntityTranslations/${id}`, translation).then((r) => r.data);
