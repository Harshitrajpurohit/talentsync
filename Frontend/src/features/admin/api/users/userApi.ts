import { api } from "../../../../shared";

import type {
  PaginationResponse,
  UserPaginationRequest,
} from "../../../../shared/types/pagination";

import type { UserRole } from "../../../../shared/types/role";
import type { UserStatus } from "../../../../shared/types/user";

import type { UserWithRole } from "../../types/user";

export interface Role {
  id: string;
  name: UserRole;
  createdAt: string;
}

interface UserStatusResponse {
  userId: string;
  status: UserStatus;
}

interface UserDeletionResponse {
  userId: string;
  isDeleted: boolean;
}

interface CreateUserRoleRequest {
  userId: string;
  roleId: string;
}

interface UserRoleResponse {
  id: string;
  userId: string;
  roleId: string;
}

export const userApi = {
  // Get users with pagination and filters
  getUsers: (
    request: UserPaginationRequest,
  ): Promise<PaginationResponse<UserWithRole>> =>
    api
      .get("/userroles", {
        params: request,
      })
      .then((response) => response.data),

  // Get all available roles
  getRoles: (): Promise<Role[]> =>
    api
      .get("/roles")
      .then((response) => response.data),

  // Change user role
  createUserRole: (
    request: CreateUserRoleRequest,
  ): Promise<UserRoleResponse> =>
    api
      .post("/userroles", request)
      .then((response) => response.data),

  // Change user status
  changeUserStatus: (
    userId: string,
    newStatus: UserStatus,
  ): Promise<UserStatusResponse> =>
    api
      .patch(`/users/${userId}/status`, null, {
        params: {
          newStatus,
        },
      })
      .then((response) => response.data),

  // Soft delete user
  deleteUser: (userId: string): Promise<void> =>
    api
      .delete(`/users/${userId}`)
      .then(() => undefined),

  // Restore soft-deleted user
  restoreUser: (
    userId: string,
  ): Promise<UserDeletionResponse> =>
    api
      .post(`/users/${userId}/restore`)
      .then((response) => response.data),
};