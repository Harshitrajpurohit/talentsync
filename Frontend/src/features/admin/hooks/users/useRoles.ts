import { useCallback, useEffect, useState } from "react";
import { userApi, type Role } from "../../api/users/userApi";

export function useRoles() {
  const [roles, setRoles] = useState<Role[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadRoles = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const result = await userApi.getRoles();

      setRoles(result);
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to load roles.",
      );
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadRoles();
  }, [loadRoles]);
  return {
    roles,
    loading,
    error,
    refetch: loadRoles,
  };
}