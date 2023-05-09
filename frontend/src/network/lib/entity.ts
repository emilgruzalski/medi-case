import apiClient from '../apiClient';
import { CreateEntityRequest, Entity, TreeNode, UpdateEntityRequest } from '../types';

export const getTree = (langId: number) =>
  apiClient.get<TreeNode[]>('/EntitiesGraphData/tree', { params: { langId } }).then((r) => r.data);

export const getEntity = (id: string) => apiClient.get<Entity>(`/Entity/${id}`).then((r) => r.data);

export const createEntity = (entity: CreateEntityRequest) =>
  apiClient.post<Entity>('/Entity', entity).then((r) => r.data);

export const deleteEntity = (id: string) => apiClient.delete(`/Entity/${id}`);

export const updateEntity = (id: string, entity: UpdateEntityRequest) =>
  apiClient.put<Entity>(`/Entity/${id}`, entity).then((r) => r.data);
