import type { ApplicationStatus } from "../../../shared/types/recruitment";

export interface Application {
  id: string;
  jobId: string;
  candidateId: string;
  submittedDate: string;
  status: ApplicationStatus;
  createdAt: string;
}

export interface ApplicationWithDetails {
  id: string;
  jobId: string;
  jobTitle: string;
  candidateId: string;
  candidateName: string;
  submittedDate: string;
  status: ApplicationStatus;
  createdAt: string;
}

export interface CreateApplicationRequest {
  jobId: string;
}

export interface ApplicationProfile {
  id: string;

  candidateId: string;
  candidateName: string;
  candidateEmail: string;
  candidatePhone?: string;
  resumeUrl?: string;

  jobId: string;
  jobTitle: string;
  department: string;

  submittedDate: string;
  status: ApplicationStatus;
  createdAt: string;
}