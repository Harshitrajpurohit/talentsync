import type { JobStatus } from "../../../shared/types/recruitment";
import type { ApplicationWithDetails } from "../../application/types/application";

export interface RecruiterDashboard {
  openJobs: number;
  totalApplications: number;
  pendingScreenings: number;
  applicationsToday: number;
  interviewsScheduled: number;
  closedJobs: number;
  recentApplications: ApplicationWithDetails[];
  recentJobs: DashboardJob[];
}

export interface DashboardJob {
  id: string;
  title: string;
  department: string;
  status: JobStatus;
  postedDate: string;
}