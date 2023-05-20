import apiClient from '../apiClient';
import { Group, PagedResult, PagingParams } from '../types';

export const getGroups = (params: PagingParams) =>
  apiClient.get<PagedResult<Group>>('/Groups', { params }).then((r) => r.data);
