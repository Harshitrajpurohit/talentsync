import { useCallback, useEffect, useState } from "react";

import { getJobSummary } from "../api/jobApi";

import type { JobSummary } from "../types/job";

export function useJobSummary(id: string) {
  const [summary, setSummary] = useState<JobSummary | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  const fetchSummary = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const data = await getJobSummary(id);

      setSummary(data);
    } catch {
      setError("Failed to load job summary.");
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    if (id) {
      fetchSummary();
    }
  }, [fetchSummary, id]);

  return {
    summary,
    loading,
    error,
    refetch: fetchSummary,
  };
}