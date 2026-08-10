import { useCallback, useEffect, useState } from "react";

import { recruiterDashboardApi } from "../api/dashboard/recruiterDashboardApi";

import type { RecruiterDashboard } from "../types/dashboard";

export function useRecruiterDashboard() {
  const [dashboard, setDashboard] =
    useState<RecruiterDashboard | null>(null);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadDashboard = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const response =
        await recruiterDashboardApi.getDashboard();

      setDashboard(response);
    } catch (err) {
      setDashboard(null);

      setError(
        err instanceof Error
          ? err.message
          : "Failed to load recruiter dashboard."
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