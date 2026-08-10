import { useState } from "react";

import { interviewApi } from "../api/interviewApi";

import type {
  InterviewResponse,
  RescheduleInterviewRequest,
} from "../types/interview";

export function useRescheduleInterview() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const rescheduleInterview = async (
    interviewId: string,
    request: RescheduleInterviewRequest,
  ): Promise<InterviewResponse | null> => {
    try {
      setLoading(true);
      setError("");

      return await interviewApi.rescheduleInterview(
        interviewId,
        request,
      );
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : "Failed to reschedule interview.",
      );

      return null;
    } finally {
      setLoading(false);
    }
  };

  return {
    rescheduleInterview,
    loading,
    error,
  };
}