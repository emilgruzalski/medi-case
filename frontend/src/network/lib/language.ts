import apiClient from '../apiClient';
import { Language } from '../types';

export const getLanguages = () => apiClient.get<Language[]>('/EntityLanguages').then((r) => r.data);
