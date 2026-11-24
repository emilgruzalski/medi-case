import apiClient from '../apiClient';
import { EntityType } from '../types';

export const getEntityTypes = () => apiClient.get<EntityType[]>('/EntityTypes').then((r) => r.data);
