import apiClient from '../apiClient';
import { CreateEntityRequest, Entity, EntityContent, TreeNode } from '../types';

export const getTree = (langId: number) =>
  apiClient.get<TreeNode[]>('/EntitiesGraphData/tree', { params: { langId } }).then((r) => r.data);

export const getEntity = (id: string) => apiClient.get<Entity>(`/Entity/${id}`).then((r) => r.data);

export const getEntityWithChildren = (id: string, langId: number) =>
  apiClient.get<EntityContent>(`/Entity/${id}/children`, { params: { langId } }).then((r) => r.data);

export const createEntity = (entity: CreateEntityRequest) =>
  apiClient.post<Entity>('/Entity', entity).then((r) => r.data);

export const deleteEntity = (id: string) => apiClient.delete(`/Entity/${id}`);
