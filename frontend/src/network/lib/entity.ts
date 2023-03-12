import apiClient from '../apiClient';
import { Entity, TreeNode } from '../types';

export const getTree = (langId: number) =>
  apiClient.get<TreeNode[]>('/EntitiesGraphData/tree', { params: { langId } }).then((r) => r.data);

export const getEntity = (id: string) => apiClient.get<Entity>(`/Entity/${id}`).then((r) => r.data);
