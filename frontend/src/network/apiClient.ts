import axios from 'axios';
import useStore from '../state/store';

const HTTP_UNAUTHORIZED = 401;

const apiClient = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

const applyToken = (token: string | null) => {
  if (token) {
    apiClient.defaults.headers.common.Authorization = `Bearer ${token}`;
  } else {
    delete apiClient.defaults.headers.common.Authorization;
  }
};

applyToken(useStore.getState().token);
useStore.subscribe((state) => applyToken(state.token));

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
