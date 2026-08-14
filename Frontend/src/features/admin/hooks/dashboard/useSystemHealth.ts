import { useCallback, useEffect, useState } from "react";

import { healthApi } from "../../api/dashboard/healthApi";
import type { SystemHealth } from "../../types/health";

export function useSystemHealth() {
  const [health, setHealth] = useState<SystemHealth | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const fetchHealth = useCallback(async () => {
    try {
      setLoading(true);
      setError(null);

      const data = await healthApi.getReadiness();

      setHealth(data);
    } catch (err) {
      console.error("Failed to fetch system health:", err);

      setHealth(null);
      setError("Unable to retrieve system health.");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchHealth();
  }, [fetchHealth]);
  
  return {
    health,
    loading,
    error,
    refetch: fetchHealth,
  };
}