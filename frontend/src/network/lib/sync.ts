import apiClient from '../apiClient';
import { PendingChanges, SynchronizationResult } from '../types';

export const getPendingChanges = (langId: number) =>
  apiClient.get<PendingChanges>('/Synchronization/changes', { params: { langId } }).then((r) => r.data);

export const publish = () => apiClient.post<SynchronizationResult>('/Synchronization').then((r) => r.data);
