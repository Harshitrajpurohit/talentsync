import axios, {
  AxiosError,
  type InternalAxiosRequestConfig,
} from "axios";
import toast from "react-hot-toast";

import { clearAuth, getToken } from "./authStorage";

// 1. Export interface at top for reuse across application
export interface ErrorResponse {
  success: boolean;
  statusCode: number;
  message: string;
  timestamp: string;
  traceId: string;
}

const BASE_URL = import.meta.env.VITE_API_URL ?? "https://localhost:5001";

export const api = axios.create({
  baseURL: `${BASE_URL}/api`,
  timeout: 30000,
  headers: {
    "Content-Type": "application/json",
  },
});

/**
 * Request Interceptor
 */
api.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const token = getToken();

    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }

    return config;
  },
  (error) => Promise.reject(error)
);

/**
 * Response Interceptor
 */
api.interceptors.response.use(
  (response) => {
    const body = response.data;

    // Unwrap standard API envelope if present
    if (
      body &&
      typeof body === "object" &&
      "data" in body &&
      body.data !== undefined &&
      !("totalCount" in body) &&
      !("page" in body)
    ) {
      response.data = body.data;
    }

    return response;
  },

  // 2. Type AxiosError generic with ErrorResponse
  (error: AxiosError<ErrorResponse>) => {
    if (!error.response) {
      toast.error("Network error. Please check your internet connection.");
      return Promise.reject(error);
    }

    const { status, data } = error.response;
    
    // Safely extract backend error message
    const serverMessage = data && typeof data === "object" ? data.message : undefined;

    switch (status) {
      case 400:
        toast.error(serverMessage ?? "Invalid request.");
        break;

      // 3. Handle 401 & clear authentication state
      case 401:
        clearAuth();
        toast.error(serverMessage ?? "Session expired. Please log in again.");
        break;

      case 403:
        toast.error(serverMessage ?? "You do not have permission to perform this action.");
        break;

      case 404:
        toast.error(serverMessage ?? "Requested resource not found.");
        break;

      default:
        toast.error(serverMessage ?? "Server error. Please try again later.");
        break;
    }

    return Promise.reject(error);
  }
);

export default api;