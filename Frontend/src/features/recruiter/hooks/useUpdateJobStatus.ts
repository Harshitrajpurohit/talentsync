import { useState } from "react";

import { updateJobStatus } from "../../jobs/api/jobApi";

import type {
  JobDetails,
} from "../../jobs/types/job";
import type { JobStatus } from "../../../shared/types/jobs";

export function useUpdateJobStatus() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string>();

  const updateStatus = async (
    id: string,
    status: JobStatus,
  ): Promise<JobDetails | undefined> => {
    try {
      setLoading(true);
      setError(undefined);

      return await updateJobStatus(id, { status });
    } catch {
      setError("Failed to update job status.");
    } finally {
      setLoading(false);
    }
  };

  return {
    updateStatus,
    loading,
    error,
  };
}