import { useEffect, useState } from "react";

import { getCandidate } from "../api/candidateApi";

import type { User } from "../../../shared/types/user";

export function useCandidate(candidateId: string) {
  const [candidate, setCandidate] = useState<User>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  useEffect(() => {
    if (!candidateId) return;

    async function loadCandidate() {
      try {
        setLoading(true);

        const response = await getCandidate(candidateId);

        setCandidate(response);
      } catch {
        setError("Failed to load candidate.");
      } finally {
        setLoading(false);
      }
    }

    loadCandidate();
  }, [candidateId]);

  return {
    candidate,
    loading,
    error,
  };
}