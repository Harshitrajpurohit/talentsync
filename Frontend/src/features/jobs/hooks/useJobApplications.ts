import { useCallback, useEffect, useState } from "react";

import { applicationApi } from "../../application/api/applicationApi";

import type { PaginationResponse } from "../../../shared/types/pagination";
import type { ApplicationWithDetails } from "../../application/types/application";

export function useJobApplications(
  jobId: string,
  pageNumber: number,
  pageSize: number,
) {
  const [applications, setApplications] =
    useState<PaginationResponse<ApplicationWithDetails> | null>(null);

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState("");

  const fetchApplications = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const data = await applicationApi.getApplicationsByJob(jobId, {
        pageNumber,
        pageSize,
      });

      setApplications(data);
    } catch {
      setError("Failed to load job applications.");
    } finally {
      setLoading(false);
    }
  }, [jobId, pageNumber, pageSize]);

  useEffect(() => {
    if (jobId) {
      fetchApplications();
    }
  }, [fetchApplications, jobId]);

  return {
    applications,
    loading,
    error,
    refetch: fetchApplications,
  };
}