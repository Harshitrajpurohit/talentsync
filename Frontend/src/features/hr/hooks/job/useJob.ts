import { useEffect, useState } from "react";

import { getJobById } from "../../api/job/jobApi";

import type { JobDetails } from "../../types/job";

export function useJob(id: string) {
  const [job, setJob] =
    useState<JobDetails>();

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string>();

  useEffect(() => {
    async function loadJob() {
      try {
        setLoading(true);
        setError(undefined);

        const response = await getJobById(id);

        setJob(response);
      } catch {
        setError("Failed to load job.");
      } finally {
        setLoading(false);
      }
    }

    if (id) {
      loadJob();
    }
  }, [id]);

  return {
    job,
    loading,
    error,
  };
}