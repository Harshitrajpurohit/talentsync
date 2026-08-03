import { useEffect, useState } from "react";

import { getCandidates } from "../api/candidateApi";

import type { PaginationResponse } from "../../../shared/types/pagination";
import type { User } from "../../../shared/types/user";

export function useCandidates(
  pageNumber: number,
  pageSize: number,
) {
  const [candidates, setCandidates] =
    useState<PaginationResponse<User>>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  useEffect(() => {
    async function loadCandidates() {
      try {
        setLoading(true);

        const response = await getCandidates({
          pageNumber,
          pageSize,
        });

        setCandidates(response);
      } catch {
        setError("Failed to load candidates.");
      } finally {
        setLoading(false);
      }
    }

    loadCandidates();
  }, [pageNumber, pageSize]);

  return {
    candidates,
    loading,
    error,
  };
}