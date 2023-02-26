import apiClient from '../apiClient';
import { LoginRequest, Session } from '../types';

export const login = (credentials: LoginRequest) =>
  apiClient.post<Session>('/Sessions/login', credentials).then((r) => r.data);

export const logout = () => apiClient.post('/Sessions/logout');
