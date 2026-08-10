import { useCallback, useEffect, useState } from "react";

import { interviewApi } from "../api/interviewApi";

import type {
  InterviewDetailed,
} from "../types/interview";

import type { InterviewPaginationRequest, PaginationResponse } from "../../../shared/types/pagination";

export function useAssignedInterviews(
  request: InterviewPaginationRequest
) {
  const [result, setResult] = useState<
    PaginationResponse<InterviewDetailed> | null
  >(null);

  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadInterviews = useCallback(async () => {
    try {
      setLoading(true);
      setError("");

      const response =
        await interviewApi.getAssignedInterviews(request);

      setResult(response);
    } catch (err) {
      setResult(null);

      setError(
        err instanceof Error
          ? err.message
          : "Failed to load interviews."
      );
    } finally {
      setLoading(false);
    }
  }, [
    request.pageNumber,
    request.pageSize,
    request.search,
    request.status,
    request.fromDate,
    request.toDate,
  ]);

  useEffect(() => {
    void loadInterviews();
  }, [loadInterviews]);

  return {
    interviews: result?.data ?? [],
    pagination: result
      ? {
          pageNumber: result.pageNumber,
          pageSize: result.pageSize,
          totalRecords: result.totalRecords,
        }
      : {
          pageNumber: request.pageNumber,
          pageSize: request.pageSize,
          totalRecords: 0,
        },
    loading,
    error,
    refetch: loadInterviews,
  };
}