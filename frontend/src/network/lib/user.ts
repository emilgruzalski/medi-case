import apiClient from '../apiClient';
import { RegisterRequest, User } from '../types';

export const register = (user: RegisterRequest) =>
  apiClient.post<User>('/Users/register', user).then((r) => r.data);

export const getCurrentUser = () => apiClient.get<User>('/Users/me').then((r) => r.data);
