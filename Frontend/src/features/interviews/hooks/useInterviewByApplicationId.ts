import { useCallback, useEffect, useState } from "react";

import { interviewApi } from "../api/interviewApi";

import type { Interview } from "../types/interview";
import type { ApplicationProfile } from "../../application/types/application";

export function useInterviewByApplicationId(
  application: ApplicationProfile | undefined
) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [interview, setInterview] = useState<Interview | null>(null);


  const loadInterview = useCallback(async () => {
    if (!application) {
      setInterview(null);
      return;
    }
    if(!(application.status === "InterviewScheduled" || application.status === "InterviewCompleted")){
      setInterview(null);
      return;
    }

    try {
      setLoading(true);
      setError("");
      console.log("before call");
      const result =
        await interviewApi.getInterviewByApplicationId(application.id);
      console.log("after call");
      console.log(result);
      setInterview(result);
    } catch (err) {
      setInterview(null);

      setError(
        err instanceof Error
          ? err.message
          : "Failed to load interview."
      );
    } finally {
      setLoading(false);
    }
  }, [application]);

  useEffect(() => {
    void loadInterview();
  }, [loadInterview]);

  return {
    interview,
    loading,
    error,
    refetch: loadInterview,
  };
}