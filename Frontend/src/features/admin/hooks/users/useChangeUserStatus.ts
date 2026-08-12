import { useState } from "react";

import { userApi } from "../../api/users/userApi";

import type { UserStatus } from "../../../../shared/types/user";

export function useChangeUserStatus() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const changeStatus = async (
    userId: string,
    status: UserStatus,
  ) => {
    try {
      setLoading(true);
      setError("");

      const result = await userApi.changeUserStatus(
        userId,
        status,
      );

      return result;
    } catch (err) {
      const message =
        err instanceof Error
          ? err.message
          : "Failed to change user status.";

      setError(message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return {
    changeStatus,
    loading,
    error,
  };
}