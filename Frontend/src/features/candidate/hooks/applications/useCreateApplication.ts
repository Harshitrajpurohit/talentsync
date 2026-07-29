import { useState } from "react";

import { applicationApi } from "../../api/applications/applicationApi";

import type { CreateApplicationRequest } from "../../types/application";

export function useCreateApplication() {
  const [loading, setLoading] = useState(false);

  const apply = async (request: CreateApplicationRequest) => {
    setLoading(true);

    try {
      return await applicationApi.createApplication(request);
    } finally {
      setLoading(false);
    }
  };

  return {
    apply,
    loading,
  };
}