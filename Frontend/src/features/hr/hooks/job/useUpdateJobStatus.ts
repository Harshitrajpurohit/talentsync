import { useState } from "react";

import { updateJobStatus } from "../../api/job/jobApi";

import type {
  JobDetails,
} from "../../types/job";
import type { JobStatus } from "../../../jobs/types/job";

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