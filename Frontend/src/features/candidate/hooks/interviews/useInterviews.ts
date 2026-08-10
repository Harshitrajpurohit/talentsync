import { useCallback, useEffect, useState } from "react";

import { interviewApi } from "../../api/interviews/interviewApi";

import type {
  PaginationResponse, InterviewPaginationRequest
} from "../../../../shared/types/pagination";

import type {
  CandidateInterview,
} from "../../types/interview";

export function useInterviews(
  pagination: InterviewPaginationRequest,
) {
  const [interviews, setInterviews] =
    useState<PaginationResponse<CandidateInterview>>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  const fetchInterviews = useCallback(async () => {
    try {
      setLoading(true);
      setError(undefined);

      const response =
        await interviewApi.getMyInterviews(
          pagination,
        );

      setInterviews(response);
    } catch {
      setError("Failed to load interviews.");
    } finally {
      setLoading(false);
    }
  }, [pagination]);

  useEffect(() => {
    fetchInterviews();
  }, [fetchInterviews]);

  return {
    interviews,
    loading,
    error,
    refetch: fetchInterviews,
  };
}