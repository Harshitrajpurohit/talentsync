import { useCallback, useEffect, useState } from "react";
import { jobsApi } from "../api/jobsApi";

import type { JobListItem } from "../types/job";
import type { PaginationResponse } from "../../../shared/types/pagination";

export function useJobs(pageNumber: number, pageSize = 10) {

  const [jobs, setJobs] = useState<JobListItem[]>([]);
  
  const [pagination, setPagination] =
    useState<PaginationResponse<JobListItem> | null>(null);

  const [loading, setLoading] = useState(false);

  const fetchJobs = useCallback(async () => {
    try {
      setLoading(true);

      const { data } = await jobsApi.getAll({
        pageNumber,
        pageSize,
      });

      setJobs(data.data);
      setPagination(data);
    } finally {
      setLoading(false);
    }
  }, [pageNumber, pageSize]);

  useEffect(() => {
    void fetchJobs();
  }, [fetchJobs]);

  return {
    jobs,
    pagination,
    loading,
    refresh: fetchJobs,
  };
}