import { useCallback, useEffect, useState } from "react";

import { adminDashboardApi } from "../../api/dashboard/adminDashboardApi";
import type { AdminDashboard } from "../../types/dashboard";

export function useAdminDashboard() {
  const [dashboard, setDashboard] = useState<AdminDashboard | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchDashboard = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await adminDashboardApi.getDashboard();

      setDashboard(data);
    } catch (err) {
      console.error("Failed to fetch admin dashboard:", err);

      setError("Failed to load dashboard data.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchDashboard();
  }, [fetchDashboard]);

  return {
    dashboard,
    loading,
    error,
    refetch: fetchDashboard,
  };
}