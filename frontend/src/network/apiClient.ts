import axios from 'axios';
import useStore from '../state/store';
import { getValidAccessToken } from '../lib/auth';

const HTTP_UNAUTHORIZED = 401;

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

apiClient.interceptors.request.use(async (config) => {
  const token = await getValidAccessToken();
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

apiClient.interceptors.response.use(
  (response) => response,
  (error) => {
    if (error.response?.status === HTTP_UNAUTHORIZED) {
      useStore.getState().clearSession();
    }
    return Promise.reject(error);
  }
);

export default apiClient;
