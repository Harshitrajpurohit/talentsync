import { useEffect, useState } from "react";

import { getCandidateInterviews } from "../../api/candidate/candidateApi";

import type {
  PaginationResponse,
} from "../../../../shared/types/pagination";

import type { InterviewDetailed } from "../../../interviews/types/interview";

export function useCandidateInterviews(
  candidateId: string,
  pageNumber: number,
  pageSize: number,
) {
  const [interviews, setInterviews] =
    useState<PaginationResponse<InterviewDetailed>>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  useEffect(() => {
    if (!candidateId) return;

    async function loadInterviews() {
      try {
        setLoading(true);

        const response = await getCandidateInterviews(
          candidateId,
          {
            pageNumber,
            pageSize,
          },
        );

        setInterviews(response);
      } catch {
        setError("Failed to load interviews.");
      } finally {
        setLoading(false);
      }
    }

    loadInterviews();
  }, [candidateId, pageNumber, pageSize]);

  return {
    interviews,
    loading,
    error,
  };
}