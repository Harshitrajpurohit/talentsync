import { getAuth } from "../../../shared/api/authStorage";
import DashboardHeader from "../components/dashboards/DashboardHeader";
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
  const authUser = getAuth()

  if (loading) {
    return <DashboardSkeleton />;
  }

  if (!dashboard) {
    return <EmptyDashboard />;
  }

  return (
    <div className="w-full space-y-6 sm:space-y-8">
      {/* 1. Header Section */}
      <DashboardHeader candidateName={authUser?.fullName} />

      {/* 2. Top-Level Metrics - Full Width */}
      <StatsGrid
        stats={{
          totalApplications: dashboard.totalApplications,
          shortlisted: dashboard.activeApplications,
          interviews: dashboard.upcomingInterviewsCount,
          offers: dashboard.selectedApplications,
        }}
      />

      {/* 3. Main Dashboard Layout - 2/3 Main Content & 1/3 Sidebar */}
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3 xl:gap-8">
        
        {/* LEFT COLUMN: Data & Tracking (Takes up 2 columns on XL screens) */}
        <div className="flex flex-col space-y-6 xl:col-span-2 xl:space-y-8">
          {/* Urgent Items First */}
          <UpcomingInterviewsCard
            interviews={dashboard.upcomingInterviews}
          />

          {/* Historical / Status Tracking Second */}
          <RecentApplicationsCard
            applications={dashboard.recentApplications}
          />
        </div>

        {/* RIGHT COLUMN: Action & Context Sidebar (Takes up 1 column on XL screens) */}
        <div className="flex flex-col space-y-6 xl:space-y-8">
          {/* Profile acts as a Sticky/High-priority Call to Action */}
          <ProfileCompletionCard
            percentage={dashboard.profileCompletion}
          />

          {/* Useful tools kept easily accessible on the right */}
          <QuickActions />
        </div>
        
      </div>
    </div>
  );
}