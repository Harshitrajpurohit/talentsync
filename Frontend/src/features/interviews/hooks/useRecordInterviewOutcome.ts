import { useState } from "react";

import { interviewApi } from "../api/interviewApi";

import type {
  InterviewResponse,
  UpdateInterviewStatusRequest,
} from "../types/interview";

export function useRecordInterviewOutcome() {
  const [loading, setLoading] = useState(false);

  async function recordOutcome(
    interviewId: string,
    request: UpdateInterviewStatusRequest
  ): Promise<InterviewResponse | null> {
    try {
      setLoading(true);

      return await interviewApi.recordOutcome(
        interviewId,
        request
      );
    } catch (err) {
      console.error(err);
      return null;
    } finally {
      setLoading(false);
    }
  }

  return {
    recordOutcome,
    loading,
  };
}