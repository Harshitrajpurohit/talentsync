import { useCallback, useEffect, useState } from "react";
import { getJobById } from "../api/jobApi";

import type { JobDetails } from "../types/job";

export function useJob(id: string) {
  const [job, setJob] = useState<JobDetails>();
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string>();

  const fetchJob = useCallback(async () => {
    try {
      setLoading(true);
      setError(undefined);

      const data = await getJobById(id);

      setJob(data);
    } catch {
      setError("Failed to load job.");
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    if (id) {
      fetchJob();
    }
  }, [fetchJob, id]);

  return {
    job,
    loading,
    error,
    refetch: fetchJob,
  };
}