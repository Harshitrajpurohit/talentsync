import api from "../../../shared/api/axios";

import type {
  CreateJobRequest,
  UpdateJobRequest,
  JobResponse,
  JobListItem,
} from "../types/job";

import type {
  PaginationRequest,
  PaginationResponse,
} from "../../../shared/types/pagination";

const BASE = "/jobs";

export const jobsApi = {
  create(request: CreateJobRequest) {
    return api.post<JobResponse>(BASE, request);
  },

  getById(id: string) {
    return api.get<JobResponse>(`${BASE}/${id}`);
  },

  getAll(params: PaginationRequest) {
    return api.get<PaginationResponse<JobListItem>>(BASE, {
      params,
    });
  },

  update(id: string, request: UpdateJobRequest) {
    return api.put<JobResponse>(`${BASE}/${id}`, request);
  },

  delete(id: string) {
    return api.delete(`${BASE}/${id}`);
  },
};