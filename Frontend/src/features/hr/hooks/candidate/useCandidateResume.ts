import { useEffect, useState } from "react";

import { getCandidateResume } from "../../api/candidate/candidateApi";

import type { Resume } from "../../types/resume";

export function useCandidateResume(candidateId: string) {
  const [resume, setResume] = useState<Resume>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  useEffect(() => {
    if (!candidateId) return;

    async function loadResume() {
      try {
        setLoading(true);

        const response = await getCandidateResume(candidateId);

        setResume(response);
      } catch {
        setError("Failed to load resume.");
      } finally {
        setLoading(false);
      }
    }

    loadResume();
  }, [candidateId]);

  return {
    resume,
    loading,
    error,
  };
}