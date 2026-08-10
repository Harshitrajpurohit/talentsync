import { UserRound } from "lucide-react";

import DashboardHeader from "../../../shared/components/dashboard/DashboardHeader";

import {
  DashboardSummary,
  RecentApplications,
  RecentJobs,
  UpcomingInterviews,
  DashboardSkeleton,
  EmptyDashboard,
} from "../components/dashboard";

import { useManagerDashboard } from "../hooks/useManagerDashboard";
import { getAuth } from "../../../shared/api/authStorage";

export default function ManagerDashboardPage() {
  const { dashboard, loading } = useManagerDashboard();
  const authUser = getAuth();

  if (loading) {
    return (
      <div className="space-y-6">
        <DashboardSkeleton />
      </div>
    );
  }

  if (!dashboard) {
    return (
      <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
        <DashboardHeader
          name={authUser?.fullName}
          description="Overview of your recruitment activities, applications, jobs, and assigned interviews."
          icon={UserRound}
        />
        <EmptyDashboard />
      </div>
    );
  }

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      
      {/* Header */}
      <DashboardHeader
        name={authUser?.fullName}
        description="Track your recruitment activities, manage applications, and stay on top of your assigned interviews."
        icon={UserRound}
      />

      {/* Summary */}
      <DashboardSummary
        openJobs={dashboard.openJobs}
        totalApplications={dashboard.totalApplications}
        interviewsToday={dashboard.interviewsToday}
        upcomingInterviews={dashboard.upcomingInterviews}
        completedInterviews={dashboard.completedInterviews}
      />

      {/* Main dashboard grid */}
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3">
        
        {/* Left Column (2/3 width on large screens) */}
        <div className="flex flex-col space-y-6 xl:col-span-2">
          <RecentApplications applications={dashboard.recentApplications} />
          <RecentJobs jobs={dashboard.recentJobs} />
        </div>

        {/* Right Column (1/3 width on large screens) */}
        <div className="flex flex-col space-y-6 xl:col-span-1">
          <UpcomingInterviews interviews={dashboard.upcomingInterviewsList} />
        </div>
        
      </div>
    </div>
  );
}