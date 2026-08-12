import { useCallback, useEffect, useState } from "react";

import { userApi } from "../../api/users/userApi";

import type {
  PaginationResponse,
  UserPaginationRequest,
} from "../../../../shared/types/pagination";

import type { UserWithRole } from "../../types/user";

export function useUsers({
  pageNumber,
  pageSize,
  search,
  role,
  status,
}: UserPaginationRequest) {
  const [users, setUsers] =
    useState<PaginationResponse<UserWithRole> | null>(null);

  const [loading, setLoading] = useState(false);

  const [error, setError] = useState("");

  const loadUsers = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const result = await userApi.getUsers({
        pageNumber,
        pageSize,
        search,
        role,
        status,
      });

      setUsers(result);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to load users.",
      );
    } finally {
      setLoading(false);
    }
  }, [
    pageNumber,
    pageSize,
    search,
    role,
    status,
  ]);

  useEffect(() => {
    void loadUsers();
  }, [loadUsers]);

  return {
    users,
    loading,
    error,
    refetch: loadUsers,
  };
}