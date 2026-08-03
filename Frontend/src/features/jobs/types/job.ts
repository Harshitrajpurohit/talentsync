import type { PaginationResponse } from "../../../shared/types/pagination";
import type { JobStatus } from "../../../shared/types/jobs";



export interface JobListItem {
  id: string;
  title: string;
  department: string;
  status: JobStatus;
  postedDate: string;
  hrId: string;
  hrName: string;
  applicationsCount: number;
}

export interface JobDetails {
  id: string;
  title: string;
  department: string;
  description: string;
  requirements: string;
  postedDate: string;
  status: JobStatus;
  hrId: string;
  hrName: string;
}

export interface CreateJobRequest {
  title: string;
  department: string;
  description: string;
  requirements: string;
}

export interface UpdateJobRequest {
  title?: string;
  department?: string;
  description?: string;
  requirements?: string;
  status?: JobStatus;
}

export interface UpdateJobStatusRequest {
  status: JobStatus;
}

export type JobListResponse = PaginationResponse<JobListItem>;

export interface JobSummary {
  totalApplications: number;
  submitted: number;
  screening: number;
  interview: number;
  selected: number;
  rejected: number;
}