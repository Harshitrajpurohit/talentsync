import { useCallback, useEffect, useState } from "react";

import { candidateJobApi } from "../../api/jobs/CandidateJobApi";

import type { CandidateJobDetails } from "../../types/job";

export function useJob(id: string) {
  const [job, setJob] = useState<CandidateJobDetails | null>(null);
  const [loading, setLoading] = useState(false);

  const loadJob = useCallback(async () => {
    if (!id) return;

    setLoading(true);

    try {
      const response = await candidateJobApi.getJobById(id);
      setJob(response);
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    void loadJob();
  }, [loadJob]);

  return {
    job,
    loading,
    refresh: loadJob,
  };
}