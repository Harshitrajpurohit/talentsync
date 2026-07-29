import { useCallback, useEffect, useState } from "react";

import { candidateJobApi } from "../../api/jobs/CandidateJobApi";

import type { CandidateJob } from "../../types/job";
import type { PaginationResponse } from "../../../../shared/types/pagination";

export function useJobs(pageNumber = 1, pageSize = 10) {
  const [jobs, setJobs] = useState<CandidateJob[]>([]);
  const [pagination, setPagination] =
    useState<PaginationResponse<CandidateJob> | null>(null);
  const [loading, setLoading] = useState(false);

  const loadJobs = useCallback(async () => {
    setLoading(true);

    try {
      const response = await candidateJobApi.getJobs(pageNumber, pageSize);

      setJobs(response.data);
      setPagination(response);
    } finally {
      setLoading(false);
    }
  }, [pageNumber, pageSize]);

  useEffect(() => {
    void loadJobs();
  }, [loadJobs]);

  return {
    jobs,
    pagination,
    loading,
    refresh: loadJobs,
  };
}