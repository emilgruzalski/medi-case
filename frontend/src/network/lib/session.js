import apiClient from '../apiClient';

export const login = (credentials) => apiClient.post('/Sessions/login', credentials).then((r) => r.data);

export const logout = () => apiClient.post('/Sessions/logout');
