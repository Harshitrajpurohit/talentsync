import { useState } from "react";

import { selectionApi } from "../api/selectionApi";

import type {
  CreateSelectionDecisionRequest,
  SelectionResponse,
} from "../types/selection";

export function useCreateSelectionDecision() {
  const [loading, setLoading] = useState(false);

  const [error, setError] = useState<string>();

  async function createSelectionDecision(
    request: CreateSelectionDecisionRequest,
  ): Promise<SelectionResponse | null> {
    try {
      setLoading(true);
      setError(undefined);

      return await selectionApi.createSelectionDecision(
        request,
      );
    } catch {
      setError("Failed to submit selection decision.");
      return null;
    } finally {
      setLoading(false);
    }
  }

  return {
    createSelectionDecision,
    loading,
    error,
  };
}