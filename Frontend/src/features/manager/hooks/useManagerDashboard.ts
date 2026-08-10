import { useCallback, useEffect, useState } from "react";

import { managerDashboardApi } from "../api/managerDashboardApi";

import type { ManagerDashboard } from "../types/dashboard";

export function useManagerDashboard() {
  const [dashboard, setDashboard] =
    useState<ManagerDashboard | null>(null);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadDashboard = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const response =
        await managerDashboardApi.getDashboard();

      setDashboard(response);
    } catch (err) {
      setDashboard(null);

      setError(
        err instanceof Error
          ? err.message
          : "Failed to load manager dashboard."
      );
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadDashboard();
  }, [loadDashboard]);

  return {
    dashboard,
    loading,
    error,
    refetch: loadDashboard,
  };
}