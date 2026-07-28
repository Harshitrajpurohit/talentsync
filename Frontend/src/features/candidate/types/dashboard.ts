
export type ApplicationStatus =
  | "Submitted"
  | "Screening"
  | "InterviewScheduled"
  | "InterviewCompleted"
  | "Selected"
  | "Rejected";

export type InterviewStatus =
  | "Scheduled"
  | "Completed"
  | "Cancelled"
  | "Rescheduled";

export interface DashboardApplication {
  id: string;
  jobId: string;
  jobTitle: string;
  status: ApplicationStatus;
  submittedDate: string;
}

export interface DashboardInterview {
  id: string;
  applicationId: string;
  jobTitle: string;
  scheduledAt: string;
  location?: string;
  interviewerName: string;
  status: InterviewStatus;
}

export interface CandidateDashboard {
  totalApplications: number;
  activeApplications: number;
  upcomingInterviewsCount: number;
  selectedApplications: number;
  profileCompletion: number;
  resumeUploaded: boolean;
  recentApplications: DashboardApplication[];
  upcomingInterviews: DashboardInterview[];
}