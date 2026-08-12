import { useState } from "react";

import { userApi } from "../../api/users/userApi";

interface ChangeUserRoleRequest {
  userId: string;
  roleId: string;
}

export function useChangeUserRole() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const changeRole = async ({
    userId,
    roleId,
  }: ChangeUserRoleRequest) => {
    try {
      setLoading(true);
      setError("");

      const result = await userApi.createUserRole({
        userId,
        roleId,
      });

      return result;
    } catch (err) {
      const message =
        err instanceof Error
          ? err.message
          : "Failed to change user role.";

      setError(message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return {
    changeRole,
    loading,
    error,
  };
}