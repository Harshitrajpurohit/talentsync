import api from "../../../../shared/api/axios";

import type {
  PaginationRequest,
  PaginationResponse,
} from "../../../../shared/types/pagination";

import type {
  CreateJobRequest,
  JobDetails,
  JobListItem,
  UpdateJobRequest,
  UpdateJobStatusRequest,
} from "../../types/job";

export async function getJobs(
  pagination: PaginationRequest,
): Promise<PaginationResponse<JobListItem>> {
  const response = await api.get<PaginationResponse<JobListItem>>(
    "/jobs",
    {
      params: pagination,
    },
  );

  return response.data;
}

export async function getJobById(
  id: string,
): Promise<JobDetails> {
  const response = await api.get<JobDetails>(
    `/jobs/${id}`,
  );

  return response.data;
}

export async function createJob(
  request: CreateJobRequest,
): Promise<JobDetails> {
  const response = await api.post<JobDetails>(
    "/jobs",
    request,
  );

  return response.data;
}

export async function updateJob(
  id: string,
  request: UpdateJobRequest,
): Promise<JobDetails> {
  const response = await api.put<JobDetails>(
    `/jobs/${id}`,
    request,
  );

  return response.data;
}

export async function updateJobStatus(
  id: string,
  request: UpdateJobStatusRequest,
): Promise<JobDetails> {
  const response = await api.patch<JobDetails>(
    `/jobs/${id}/status`,
    request,
  );

  return response.data;
}