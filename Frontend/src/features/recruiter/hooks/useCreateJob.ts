import { useState } from "react";

import { createJob } from "../../jobs/api/jobApi";

import type {
  CreateJobRequest,
  JobDetails,
} from "../../jobs/types/job";

export function useCreateJob() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>();

  const create = async (
    request: CreateJobRequest,
  ): Promise<JobDetails | undefined> => {
    try {
      setLoading(true);
      setError(undefined);

      return await createJob(request);
    } catch {
      setError("Failed to create job.");
    } finally {
      setLoading(false);
    }
  };

  return {
    create,
    loading,
    error,
  };
}