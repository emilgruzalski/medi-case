import apiClient from '../apiClient';
import { Group, GroupRequest, PagedResult, PagingParams } from '../types';

export const getGroups = (params: PagingParams) =>
  apiClient.get<PagedResult<Group>>('/Groups', { params }).then((r) => r.data);

export const createGroup = (group: GroupRequest) => apiClient.post<Group>('/Groups', group).then((r) => r.data);

export const updateGroup = (id: number, group: GroupRequest) =>
  apiClient.put<Group>(`/Groups/${id}`, group).then((r) => r.data);

export const deleteGroup = (id: number) => apiClient.delete(`/Groups/${id}`);
