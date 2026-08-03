import { useCallback, useEffect, useState } from "react";

import { getJobs } from "../api/jobApi";

import type { PaginationResponse } from "../../../shared/types/pagination";
import type { JobListItem } from "../types/job";

export function useJobs(
  pageNumber: number,
  pageSize: number,
) {
  const [jobs, setJobs] =
    useState<PaginationResponse<JobListItem>>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  const loadJobs = useCallback(async () => {
    try {
      setLoading(true);
      setError(undefined);

      const response = await getJobs({
        pageNumber,
        pageSize,
      });

      setJobs(response);
    } catch {
      setError("Failed to load jobs.");
    } finally {
      setLoading(false);
    }
  }, [pageNumber, pageSize]);

  useEffect(() => {
    loadJobs();
  }, [loadJobs]);

  return {
    jobs,
    loading,
    error,
    refetch: loadJobs,
  };
}