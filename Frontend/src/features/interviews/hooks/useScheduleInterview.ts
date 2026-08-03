import { useState } from "react";

import { interviewApi } from "../api/interviewApi";

import type {
  InterviewResponse,
  ScheduleInterviewRequest,
} from "../types/interview";

export function useScheduleInterview() {
  const [loading, setLoading] = useState(false);

  const [error, setError] = useState<string>();

  async function scheduleInterview(
    request: ScheduleInterviewRequest,
  ): Promise<InterviewResponse | null> {
    try {
      setLoading(true);
      setError(undefined);

      return await interviewApi.scheduleInterview(
        request,
      );
    } catch {
      setError("Failed to schedule interview.");
      return null;
    } finally {
      setLoading(false);
    }
  }

  return {
    scheduleInterview,
    loading,
    error,
  };
}