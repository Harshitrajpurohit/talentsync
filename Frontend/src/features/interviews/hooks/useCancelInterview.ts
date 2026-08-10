import { useState } from "react";

import { interviewApi } from "../api/interviewApi";

import type {
  InterviewResponse,
  UpdateInterviewStatusRequest,
} from "../types/interview";

export function useCancelInterview() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const cancelInterview = async (
    interviewId: string,
    request: UpdateInterviewStatusRequest,
  ): Promise<InterviewResponse | null> => {
    try {
      setLoading(true);
      setError("");

      return await interviewApi.cancelInterview(
        interviewId,
        request,
      );
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to cancel interview.",
      );

      return null;
    } finally {
      setLoading(false);
    }
  };

  return {
    cancelInterview,
    loading,
    error,
  };
}