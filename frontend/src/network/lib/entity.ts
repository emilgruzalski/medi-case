import apiClient from '../apiClient';
import { TreeNode } from '../types';

export const getTree = (langId: number) =>
  apiClient.get<TreeNode[]>('/EntitiesGraphData/tree', { params: { langId } }).then((r) => r.data);
