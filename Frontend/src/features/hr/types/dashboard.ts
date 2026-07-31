import type { ApplicationWithDetails } from "../../application/types/application";
import type { InterviewDetailed } from "../../interviews/types/interview";
import type { JobResponse } from "../../jobs/types/job";

export type HrDashboard = {
  totalJobs: number;
  openJobs: number;
  totalCandidates: number;
  totalApplications: number;
  interviewsToday: number;

  recentApplications: ApplicationWithDetails[];
  upcomingInterviews: InterviewDetailed[];
  recentJobs: JobResponse[];
};