import apiClient from '../apiClient';
import { TranslationFile } from '../types';

/** Downloads a file with the session token and returns a local object URL for it. */
export const getFileObjectUrl = (fileId: string) =>
  apiClient
    .get<Blob>(`/EntityTranslationFiles/${fileId}/download`, { responseType: 'blob' })
    .then((r) => URL.createObjectURL(r.data));

export const uploadFile = (translationId: string, file: File) => {
  const form = new FormData();
  form.append('translationId', translationId);
  form.append('file', file);
  return apiClient.post<TranslationFile>('/EntityTranslationFiles', form).then((r) => r.data);
};

export const deleteFile = (id: string) => apiClient.delete(`/EntityTranslationFiles/${id}`);
