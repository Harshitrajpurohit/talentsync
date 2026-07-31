import { useEffect, useState } from "react";

import { applicationApi } from "../../api/applications/applicationApi";

import type { ApplicationWithDetails } from "../../../application/types/application";

export function useApplications() {
  const [applications, setApplications] = useState<ApplicationWithDetails[]>([]);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    void loadApplications();
  }, []);

  async function loadApplications() {
    setLoading(true);

    try {
      const response = await applicationApi.getApplications();
      setApplications(response);
    } finally {
      setLoading(false);
    }
  }

  return {
    applications,
    loading,
    refresh: loadApplications,
  };
}