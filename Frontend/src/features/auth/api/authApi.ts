import { api } from "../../../shared";
import type {
  LoginRequest,
  LoginResponse,
  RegisterRequest,
} from "../types";

export const authApi = {
  login: (data: LoginRequest) =>
    api.post<LoginResponse>("/auth/login", data).then((r) => r.data),

  register: (data: RegisterRequest) =>
    api.post("/auth/register", data).then((r) => r.data),

  logout: () =>
    api.post("/auth/logout").then((r) => r.data),

  refreshToken: () =>
    api.post("/auth/refresh").then((r) => r.data),
};