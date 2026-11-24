import apiClient from '../apiClient';
import { Translation, TranslationFile } from '../types';

export const translateWithAi = (sourceTranslationId: string, targetLangId: number) =>
  apiClient.post<Translation>('/Ai/translate', { sourceTranslationId, targetLangId }).then((r) => r.data);

export const generateSpeech = (translationId: string) =>
  apiClient.post<TranslationFile>(`/Ai/speech/${translationId}`).then((r) => r.data);
