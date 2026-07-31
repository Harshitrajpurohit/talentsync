import type { InterviewStatus } from "../../../shared/types/interviewStatus";

export interface ScheduleInterviewRequest {
  applicationId: string;
  scheduledAt: string;
  interviewerId: string;
  location?: string;
}

export interface Interview {
  id: string;
  applicationId: string;
  scheduledAt: string;
  location?: string;
  interviewerId: string;
  interviewerName: string;
  status: InterviewStatus;
  feedback?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface InterviewDetailed {
  id: string;
  applicationId: string;
  jobId: string;
  jobTitle: string;
  candidateId: string;
  candidateName: string;
  candidateEmail: string;
  scheduledAt: string;
  location?: string;
  interviewerId: string;
  interviewerName: string;
  status: InterviewStatus;
  feedback?: string;
  createdAt: string;
  updatedAt?: string;
}

export interface UpdateInterviewStatusRequest {
  status: InterviewStatus;
  feedback?: string;
}

export interface RescheduleInterviewRequest {
  scheduledAt: string;
  location: string;
}