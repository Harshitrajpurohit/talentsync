import { BriefcaseBusiness } from "lucide-react";

import DashboardHeader from "../../../shared/components/dashboard/DashboardHeader";

import {
  RecruiterDashboardEmpty,
  RecruiterDashboardSkeleton,
  RecruiterStatsGrid,
  RecentApplicationsCard,
  RecentJobsCard,
} from "../components/dashboard";

import { useRecruiterDashboard } from "../hooks/useRecruiterDashboard";

import { getAuth } from "../../../shared/api/authStorage";

export default function RecruiterDashboardPage() {
  const { dashboard, loading, error } =
    useRecruiterDashboard();

  const user = getAuth();

  if (loading) {
    return <RecruiterDashboardSkeleton />;
  }

  if (error || !dashboard) {
    return (
      <RecruiterDashboardEmpty
        message={error || "Unable to load recruiter dashboard."}
      />
    );
  }

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      <DashboardHeader
        name={user?.fullName}
        description="Manage job postings, monitor applications, and track recruitment activities from one place."
        icon={BriefcaseBusiness}
      />

      <RecruiterStatsGrid dashboard={dashboard} />

      <div className="grid gap-6 xl:grid-cols-2">
        <RecentApplicationsCard
          applications={dashboard.recentApplications}
        />

        <RecentJobsCard jobs={dashboard.recentJobs} />
      </div>
    </div>
  );
}