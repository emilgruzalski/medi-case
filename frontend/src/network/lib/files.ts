import apiClient from '../apiClient';

/** Downloads a file with the session token and returns a local object URL for it. */
export const getFileObjectUrl = (fileId: string) =>
  apiClient
    .get<Blob>(`/EntityTranslationFiles/${fileId}/download`, { responseType: 'blob' })
    .then((r) => URL.createObjectURL(r.data));
