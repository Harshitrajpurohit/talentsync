import { useState } from "react";

import { jobsApi } from "../api/jobsApi";

import type {
  CreateJobRequest,
  JobResponse,
} from "../types/job";

export function useCreateJob() {
  const [loading, setLoading] = useState(false);

  async function createJob(
    request: CreateJobRequest
  ): Promise<JobResponse> {
    setLoading(true);

    try {
      const response = await jobsApi.create(request);

      return response.data;
    } finally {
      setLoading(false);
    }
  }

  return {
    createJob,
    loading,
  };
}