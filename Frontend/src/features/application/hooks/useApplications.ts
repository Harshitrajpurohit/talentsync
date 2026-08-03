import {
  useCallback,
  useEffect,
  useState,
} from "react";

import { applicationApi } from "../../application/api/applicationApi";

import type {
  PaginationResponse,
  ApplicationPaginationRequest,
} from "../../../shared/types/pagination";

import type {
  ApplicationStatus,
} from "../../../shared/types/recruitment";

import type {
  ApplicationWithDetails,
} from "../../application/types/application";

export function useApplications(
  pageNumber: number,
  pageSize: number,
  search?: string,
  status?: ApplicationStatus,
  jobId?: string,
) {
  const [applications, setApplications] =
    useState<
      PaginationResponse<ApplicationWithDetails>
    >();

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string>();

  const fetchApplications =
    useCallback(async () => {
      try {
        setLoading(true);
        setError(undefined);

        const request: ApplicationPaginationRequest =
          {
            pageNumber,
            pageSize,
            search:
              search && search.trim().length >= 2
                ? search.trim()
                : undefined,
            status,
            jobId,
          };

        const response =
          await applicationApi.getApplications(
            request,
          );

        setApplications(response);
      } catch {
        setError(
          "Failed to load applications.",
        );
      } finally {
        setLoading(false);
      }
    }, [
      pageNumber,
      pageSize,
      search,
      status,
      jobId,
    ]);

  useEffect(() => {
    fetchApplications();
  }, [fetchApplications]);

  return {
    applications,
    loading,
    error,
    refetch: fetchApplications,
  };
}