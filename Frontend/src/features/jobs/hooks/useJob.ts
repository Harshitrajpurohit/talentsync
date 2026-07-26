import { useCallback, useEffect, useState } from "react";

import { jobsApi } from "../api/jobsApi";

import type { Job } from "../types";

export function useJob(id: string) {
  const [job, setJob] = useState<Job | null>(null);
  const [loading, setLoading] = useState(false);

  const fetchJob = useCallback(async () => {
    if (!id) return;

    setLoading(true);

    try {
      const response = await jobsApi.getById(id);

      setJob(response.data);
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    void fetchJob();
  }, [fetchJob]);

  return {
    job,
    loading,
    refresh: fetchJob,
  };
}