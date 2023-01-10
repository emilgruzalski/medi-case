import apiClient from '../apiClient';

export const register = (user) => apiClient.post('/Users/register', user).then((r) => r.data);

export const getCurrentUser = () => apiClient.get('/Users/me').then((r) => r.data);
