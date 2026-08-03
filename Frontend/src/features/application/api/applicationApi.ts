import { api } from "../../../shared";


import type {
  ApplicationPaginationRequest,
  PaginationRequest,
  PaginationResponse,
} from "../../../shared/types/pagination";
import type { ApplicationStatus } from "../../../shared/types/recruitment";

import type {
  ApplicationProfile,
  ApplicationWithDetails,
} from "../types/application";

export const applicationApi = {

  getApplications: (
    pagination: ApplicationPaginationRequest,
  ) =>
    api
      .get<PaginationResponse<ApplicationWithDetails>>(
        "/applications",
        {
          params: pagination,
        },
      )
      .then((r) => r.data),


  getApplicationById: (
    id: string,
  ) =>
    api
      .get<ApplicationProfile>(
        `/applications/${id}`,
      )
      .then((r) => r.data),
      

  getApplicationsByJob: (
    jobId: string,
    pagination: PaginationRequest,
  ) =>
    api
      .get<PaginationResponse<ApplicationWithDetails>>(
        `/applications/job/${jobId}`,
        {
          params: pagination,
        },
      )
      .then((response) => response.data),

    updateApplicationStatus: (
    id: string,
    request: ApplicationStatus,
  ) =>
    api
      .patch<ApplicationWithDetails>(
        `/applications/${id}/status`,
        request,
      )
      .then((r) => r.data),

};