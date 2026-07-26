import { useState } from "react";

import { jobsApi } from "../api/jobsApi";

import type {
  UpdateJobRequest,
  JobResponse,
} from "../types";

export function useUpdateJob() {
  const [loading, setLoading] = useState(false);

  async function updateJob(
    id: string,
    request: UpdateJobRequest
  ): Promise<JobResponse> {
    setLoading(true);

    try {
      const response = await jobsApi.update(id, request);

      return response.data;
    } finally {
      setLoading(false);
    }
  }

  return {
    updateJob,
    loading,
  };
}