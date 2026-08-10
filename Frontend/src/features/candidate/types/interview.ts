import type { InterviewStatus } from "../../../shared/types/recruitment";

export interface CandidateInterview {
  id: string;
  jobTitle: string;
  scheduledAt: string;
  location?: string;
  interviewerName: string;
  status: InterviewStatus;
  createdAt: string;
}