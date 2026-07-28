import { useEffect, useState } from "react";

import { dashboardApi } from "../../api/dashboards/dashboardApi";

import type { CandidateDashboard } from "../../types/dashboard";

export function useDashboard() {
  const [dashboard, setdashboard] = useState<CandidateDashboard | null>(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    void loadProfile();
  }, []);

  async function loadProfile() {
    setLoading(true);

    try {
      const profile = await dashboardApi.getDashboard();
      setdashboard(profile);
    } finally {
      setLoading(false);
    }
  }

  return {
    dashboard,
    loading,
    refresh: loadProfile,
  };
}