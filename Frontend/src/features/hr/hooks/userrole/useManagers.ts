import { useEffect, useState } from "react";

import { getUserRolesByThereRoleName } from "../../api/userrole/userRoleApi";

import type { UserRoleResponseWithExtra } from "../../types/userrole";

export function useManagers() {
  const [managers, setManagers] =
    useState<Array<UserRoleResponseWithExtra>>();

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string>();

  useEffect(() => {
    async function loadManagers() {
      try {
        setLoading(true);
        setError(undefined);

        const response = await getUserRolesByThereRoleName("Manager");

        setManagers(response);
      } catch {
        setError("Failed to load employees.");
      } finally {
        setLoading(false);
      }
    }

    loadManagers();
  },[]);

  return {
    managers,
    loading,
    error,
  };
}