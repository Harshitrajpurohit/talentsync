import type { JobStatus } from "../../../shared/types/recruitment";

export interface CandidateJob {
  id: string;
  title: string;
  department: string;
  postedDate: string;
  status: JobStatus;
  hasApplied: boolean;
}

export interface CandidateJobDetails {
  id: string;
  title: string;
  department: string;
  description: string;
  requirements: string;
  postedDate: string;
  status: JobStatus;
  hasApplied: boolean;
}
