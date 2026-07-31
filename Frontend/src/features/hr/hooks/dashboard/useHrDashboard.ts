import { useCallback, useEffect, useState } from "react";

import { hrDashboardApi } from "../../api/dashboard/hrDashboardApi";

import type { HrDashboard } from "../../types/dashboard";

export function useHrDashboard() {
  const [dashboard, setDashboard] =
    useState<HrDashboard | null>(null);

  const [loading, setLoading] = useState(false);

  const loadDashboard = useCallback(async () => {
    setLoading(true);

    try {
      const response = await hrDashboardApi.getDashboard();
      setDashboard(response);
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
    refresh: loadDashboard,
  };
}