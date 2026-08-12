import { useState } from "react";

import { userApi } from "../../api/users/userApi";

export function useUserDeletion() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const deleteUser = async (userId: string) => {
    try {
      setLoading(true);
      setError("");

      await userApi.deleteUser(userId);
    } catch (err) {
      const message =
        err instanceof Error
          ? err.message
          : "Failed to delete user.";

      setError(message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  const restoreUser = async (userId: string) => {
    try {
      setLoading(true);
      setError("");

      const result = await userApi.restoreUser(userId);

      return result;
    } catch (err) {
      const message =
        err instanceof Error
          ? err.message
          : "Failed to restore user.";

      setError(message);
      throw err;
    } finally {
      setLoading(false);
    }
  };

  return {
    deleteUser,
    restoreUser,
    loading,
    error,
  };
}