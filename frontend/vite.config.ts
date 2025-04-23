/// <reference types="vitest/config" />
import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';

// Same port as with Create React App, the API allows this origin.
const DEV_SERVER_PORT = 3000;

export default defineConfig({
  plugins: [react()],
  server: {
    port: DEV_SERVER_PORT,
  },
  build: {
    rollupOptions: {
      output: {
        // Libraries change less often than the app, so browsers can keep them cached between releases.
        manualChunks: {
          react: ['react', 'react-dom', 'react-router'],
          mui: ['@mui/material', '@emotion/react', '@emotion/styled'],
        },
      },
    },
  },
  test: {
    environment: 'jsdom',
    globals: true,
    setupFiles: './src/setupTests.js',
  },
});
