import apiClient from '../apiClient';
import { PagedResult, PagingParams, UpdateUserRequest, User, UserAdminRequest } from '../types';

export const getCurrentUser = () => apiClient.get<User>('/Users/me').then((r) => r.data);

export const updateCurrentUser = (user: UpdateUserRequest) =>
  apiClient.put<User>('/Users/me', user).then((r) => r.data);

export const getUsers = (params: PagingParams) =>
  apiClient.get<PagedResult<User>>('/Users', { params }).then((r) => r.data);

export const createUser = (user: UserAdminRequest) => apiClient.post<User>('/Users', user).then((r) => r.data);

export const updateUser = (id: number, user: UserAdminRequest) =>
  apiClient.put<User>(`/Users/${id}`, user).then((r) => r.data);

export const deleteUser = (id: number) => apiClient.delete(`/Users/${id}`);
