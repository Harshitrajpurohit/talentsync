import { useState } from "react";

import { updateJob } from "../../api/job/jobApi";

import type {
  JobDetails,
  UpdateJobRequest,
} from "../../types/job";

export function useUpdateJob() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>();

  const update = async (
    id: string,
    request: UpdateJobRequest,
  ): Promise<JobDetails | undefined> => {
    try {
      setLoading(true);
      setError(undefined);

      return await updateJob(id, request);
    } catch {
      setError("Failed to update job.");
    } finally {
      setLoading(false);
    }
  };

  return {
    update,
    loading,
    error,
  };
}