import { useState } from "react";

import { screeningApi } from "../api/screeningApi";

import type {
  CreateScreeningRequest,
  ScreeningResponse,
} from "../types/screening";

export function useCreateScreening() {
  const [loading, setLoading] = useState(false);

  const [error, setError] = useState<string>();

  async function createScreening(
    request: CreateScreeningRequest,
  ): Promise<ScreeningResponse | null> {
    try {
      setLoading(true);
      setError(undefined);

      return await screeningApi.createScreening(
        request,
      );
    } catch {
      setError("Failed to create screening.");
      return null;
    } finally {
      setLoading(false);
    }
  }

  return {
    createScreening,
    loading,
    error,
  };
}