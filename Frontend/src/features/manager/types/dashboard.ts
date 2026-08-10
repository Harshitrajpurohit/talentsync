import type { InterviewDetailed } from "../../interviews/types/interview";
import type { ApplicationWithDetails } from "../../application/types/application";
import type { DashboardJob } from "../../recruiter/types/dashboard";


export interface ManagerDashboard {
  openJobs: number;
  totalApplications: number;
  interviewsToday: number;
  upcomingInterviews: number;
  completedInterviews: number;

  upcomingInterviewsList: InterviewDetailed[];
  recentApplications: ApplicationWithDetails[];
  recentJobs: DashboardJob[];
}