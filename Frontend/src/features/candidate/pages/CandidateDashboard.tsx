import { UserCircle2 } from "lucide-react";

import { getAuth } from "../../../shared/api/authStorage";
import DashboardHeader from "../../../shared/components/dashboard/DashboardHeader";

import DashboardSkeleton from "../components/dashboards/DashboardSkeleton";
import EmptyDashboard from "../components/dashboards/EmptyDashboard";
import ProfileCompletionCard from "../components/dashboards/ProfileCompletionCard";
import QuickActions from "../components/dashboards/QuickActions";
import RecentApplicationsCard from "../components/dashboards/RecentApplicationsCard";
import StatsGrid from "../components/dashboards/StatsGrid";
import UpcomingInterviewsCard from "../components/dashboards/UpcomingInterviewsCard";

import { useDashboard } from "../hooks/dashboard/useDashboard";

export default function CandidateDashboardPage() {
  const { dashboard, loading } = useDashboard();
  const authUser = getAuth();

  if (loading) {
    return <DashboardSkeleton />;
  }

  if (!dashboard) {
    return (
      <div className="w-full space-y-6 sm:space-y-8">
        <DashboardHeader
          name={authUser?.fullName}
          icon={UserCircle2}
          description="Track your applications, prepare for upcoming interviews, and manage your career profile."
        />

        <EmptyDashboard />
      </div>
    );
  }

  return (
    <div className="w-full space-y-6 sm:space-y-8">
      <DashboardHeader
        name={authUser?.fullName}
        icon={UserCircle2}
        description="Track your applications, prepare for upcoming interviews, and manage your career profile."
      />

      <StatsGrid
        stats={{
          totalApplications: dashboard.totalApplications,
          shortlisted: dashboard.activeApplications,
          interviews: dashboard.upcomingInterviewsCount,
          offers: dashboard.selectedApplications,
        }}
      />

      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3 xl:gap-8">
        <div className="flex flex-col space-y-6 xl:col-span-2 xl:space-y-8">
          <UpcomingInterviewsCard
            interviews={dashboard.upcomingInterviews}
          />

          <RecentApplicationsCard
            applications={dashboard.recentApplications}
          />
        </div>

        <div className="flex flex-col space-y-6 xl:space-y-8">
          <ProfileCompletionCard
            percentage={dashboard.profileCompletion}
          />

          <QuickActions />
        </div>
      </div>
    </div>
  );
}