import apiClient from '../apiClient';
import { ChangePasswordRequest, PagedResult, PagingParams, RegisterRequest, UpdateUserRequest, User } from '../types';

export const register = (user: RegisterRequest) =>
  apiClient.post<User>('/Users/register', user).then((r) => r.data);

export const getCurrentUser = () => apiClient.get<User>('/Users/me').then((r) => r.data);

export const updateCurrentUser = (user: UpdateUserRequest) =>
  apiClient.put<User>('/Users/me', user).then((r) => r.data);

export const changePassword = (request: ChangePasswordRequest) => apiClient.put('/Users/me/password', request);

export const getUsers = (params: PagingParams) =>
  apiClient.get<PagedResult<User>>('/Users', { params }).then((r) => r.data);
