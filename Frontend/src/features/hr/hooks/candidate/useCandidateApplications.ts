import { useEffect, useState } from "react";

import { getCandidateApplications } from "../../api/candidate/candidateApi";

import type {
  PaginationRequest,
  PaginationResponse,
} from "../../../../shared/types/pagination";

import type { ApplicationWithDetails } from "../../../application/types/application";

export function useCandidateApplications(
  candidateId: string,
  pageNumber: number,
  pageSize: number,
) {
  const [applications, setApplications] =
    useState<PaginationResponse<ApplicationWithDetails>>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  useEffect(() => {
    if (!candidateId) return;

    async function loadApplications() {
      try {
        setLoading(true);

        const response = await getCandidateApplications(
          candidateId,
          {
            pageNumber,
            pageSize,
          },
        );

        setApplications(response);
      } catch {
        setError("Failed to load applications.");
      } finally {
        setLoading(false);
      }
    }

    loadApplications();
  }, [candidateId, pageNumber, pageSize]);

  return {
    applications,
    loading,
    error,
  };
}