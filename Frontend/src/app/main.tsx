import { StrictMode } from 'react';
import { createRoot } from 'react-dom/client';
import App from './App.tsx';
import { Toaster } from "react-hot-toast";
import { env } from '../config/index.ts';
import { AuthProvider } from './context/AuthContext.tsx';
import "../index.css"
void env;

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <AuthProvider>
    <App />
    <Toaster
        position="top-right"
        toastOptions={{
          duration: 3000,
          style: { borderRadius: "8px", fontFamily: "sans-serif" },
        }}
      />
      </AuthProvider>
  </StrictMode>,
);
