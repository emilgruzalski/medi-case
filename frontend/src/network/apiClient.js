import axios from 'axios';
import useStore from '../state/store';

const HTTP_UNAUTHORIZED = 401;

const apiClient = axios.create({
  baseURL: process.env.REACT_APP_API_URL,
});

apiClient.interceptors.request.use((config) => {
  const { token } = useStore.getState();
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
