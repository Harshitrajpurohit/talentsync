import { Users } from "lucide-react";

import DashboardHeader from "../../../shared/components/dashboard/DashboardHeader";

import DashboardSummary from "../components/dashboard/DashboardSummary";
import RecentApplications from "../components/dashboard/RecentApplications";
import UpcomingInterviews from "../components/dashboard/UpcomingInterviews";
import RecentJobs from "../components/dashboard/RecentJobs";
import DashboardSkeleton from "../components/dashboard/DashboardSkeleton";
import EmptyDashboard from "../components/dashboard/EmptyDashboard";

import { useHrDashboard } from "../hooks/dashboard/useHrDashboard";
import { getAuth } from "../../../shared/api/authStorage";

export default function HrDashboardPage() {
  const { dashboard, loading } = useHrDashboard();
  const authUser = getAuth();

  if (loading) {
    return (
      <div className="w-full space-y-6 sm:space-y-8">
        <div className="h-24 w-full animate-pulse rounded-[20px] bg-[#E5EAE7] dark:bg-[#1e3329]" />
        <DashboardSkeleton />
      </div>
    );
  }

  if (!dashboard) {
    return (
      <div className="w-full animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both space-y-6 sm:space-y-8">
        <DashboardHeader
          name={authUser?.fullName}
          icon={Users}
          description="Monitor recruitment activities, review recent applications, and track your hiring pipeline."
        />

        <EmptyDashboard />
      </div>
    );
  }

  return (
    <div className="w-full animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both space-y-6 sm:space-y-8">
      <DashboardHeader
        name={authUser?.fullName}
        icon={Users}
        description="Monitor recruitment activities, review recent applications, and track your hiring pipeline."
      />

      <DashboardSummary
        totalJobs={dashboard.totalJobs}
        openJobs={dashboard.openJobs}
        totalCandidates={dashboard.totalCandidates}
        totalApplications={dashboard.totalApplications}
        interviewsToday={dashboard.interviewsToday}
      />

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3 xl:gap-8">
        <div className="flex flex-col space-y-6 xl:col-span-2 xl:space-y-8">
          <RecentApplications
            applications={dashboard.recentApplications}
          />

          <RecentJobs jobs={dashboard.recentJobs} />
        </div>

        <div className="flex flex-col space-y-6 xl:col-span-1 xl:space-y-8">
          <UpcomingInterviews
            interviews={dashboard.upcomingInterviews}
          />
        </div>
      </div>
    </div>
  );
}