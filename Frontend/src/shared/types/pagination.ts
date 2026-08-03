import type { ApplicationStatus } from "./recruitment";

export interface PaginationRequest {
  pageNumber: number;
  pageSize: number;
}

export interface PaginationResponse<T> {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  data: T[];
}

export interface ApplicationPaginationRequest {
  pageNumber: number;
  pageSize: number;
  search?: string;
  status?: ApplicationStatus;
  jobId?: string;
}