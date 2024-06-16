import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// Same port as with Create React App, the API allows this origin.
const DEV_SERVER_PORT = 3000;

export default defineConfig({
  plugins: [react()],
  server: {
    port: DEV_SERVER_PORT,
  },
});
