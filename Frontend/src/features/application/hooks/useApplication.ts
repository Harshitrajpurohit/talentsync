import { useCallback, useEffect, useState } from "react";

import { applicationApi } from "../../application/api/applicationApi";

import type { ApplicationProfile } from "../../application/types/application";

export function useApplication(id: string) {
  const [application, setApplication] =
    useState<ApplicationProfile>();

  const [loading, setLoading] = useState(true);

  const [error, setError] = useState<string>();

  const fetchApplication = useCallback(async () => {
    if (!id) {
      return;
    }

    try {
      setLoading(true);
      setError(undefined);

      const response =
        await applicationApi.getApplicationById(id);

      setApplication(response);
    } catch {
      setError("Failed to load application.");
    } finally {
      setLoading(false);
    }
  }, [id]);

  useEffect(() => {
    fetchApplication();
  }, [fetchApplication]);

  return {
    application,
    loading,
    error,
    refetch: fetchApplication,
  };
}