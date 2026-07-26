import { useState } from "react";

import { jobsApi } from "../api/jobsApi";

export function useDeleteJob() {
  const [loading, setLoading] = useState(false);

  async function deleteJob(id: string): Promise<void> {
    setLoading(true);

    try {
      await jobsApi.delete(id);
    } finally {
      setLoading(false);
    }
  }

  return {
    deleteJob,
    loading,
  };
}