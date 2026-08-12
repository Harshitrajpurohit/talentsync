import type { ApplicationStatus, InterviewStatus } from "./recruitment";
import type { UserRole } from "./role";
import type { UserStatus } from "./user";

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

export interface InterviewPaginationRequest {
  pageNumber: number;
  pageSize: number;
  search?: string;
  status?: InterviewStatus;
  fromDate?: string;
  toDate?: string;
}

export interface UserPaginationRequest {
  pageNumber: number;
  pageSize: number;
  search?: string;
  status?: UserStatus;
  role?: UserRole;
}