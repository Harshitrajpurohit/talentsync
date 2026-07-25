import axios, {
  AxiosError,
  type InternalAxiosRequestConfig,
} from "axios";
import toast from "react-hot-toast";

// Import your auth helpers (adjust paths if necessary)
import { clearAuth, getAuth, saveAuth, getToken } from "./authStorage";
import { authApi } from "../../features/auth/api/authApi"; // Adjust import to your API service file

// 1. Interfaces for API Responses
export interface ErrorResponse {
  success: boolean;
  statusCode: number;
  message: string;
  timestamp: string;
  traceId: string;
}

export interface RefreshTokenResponse {
  Token: string;
  Email: string;
  Role: string;
}

// Custom request config type for retry tracking
interface RetryRequestConfig extends InternalAxiosRequestConfig {
  _retry?: boolean;
}

// Token refresh concurrency controls
let isRefreshing = false;
let refreshPromise: Promise<RefreshTokenResponse> | null = null;

const BASE_URL = import.meta.env.VITE_API_URL ?? "https://localhost:5001";

export const api = axios.create({
  baseURL: `${BASE_URL}/api`,
  timeout: 30000,
  withCredentials: true,
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
  (error: unknown) => Promise.reject(error)
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

  async (error: AxiosError<ErrorResponse>) => {
    if (!error.response) {
      toast.error("Network error. Please check your internet connection.");
      return Promise.reject(error);
    }

    const originalRequest = error.config as RetryRequestConfig;
    const { status, data } = error.response;

    // Safely extract backend error message
    const serverMessage = data && typeof data === "object" ? data.message : undefined;

    // Handle 401 & Silent Token Refresh
    if (
      status === 401 &&
      !originalRequest._retry &&
      !originalRequest.url?.includes("/auth/login") &&
      !originalRequest.url?.includes("/auth/register") &&
      !originalRequest.url?.includes("/auth/refresh")
    ) {
      originalRequest._retry = true;

      try {
        if (!isRefreshing || !refreshPromise) {
          isRefreshing = true;

          refreshPromise = authApi.refreshToken().finally(() => {
            isRefreshing = false;
            refreshPromise = null;
          });
        }

        // Store active promise locally so TypeScript knows it is non-null
        const activeRefreshPromise = refreshPromise;
        if (!activeRefreshPromise) {
          throw new Error("Failed to initialize refresh token process.");
        }

        const refresh = await activeRefreshPromise;

        const auth = getAuth();

        if (auth) {
          const updated = {
            ...auth,
            token: refresh.Token,
            email: refresh.Email,
            role: refresh.Role,
          };

          saveAuth(updated);

          if (originalRequest.headers) {
            originalRequest.headers.Authorization = `Bearer ${refresh.Token}`;
          }
        }

        return api(originalRequest);
      } catch (refreshError: unknown) {
        clearAuth();
        window.location.href = "/login";
        toast.error("Session expired. Please log in again.");
        return Promise.reject(refreshError);
      }
    }

    // Handle non-401 HTTP errors
    switch (status) {
      case 400:
        toast.error(serverMessage ?? "Invalid request.");
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