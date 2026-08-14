import { ShieldCheck } from "lucide-react";

import DashboardHeader from "../../../shared/components/dashboard/DashboardHeader";

import {
  DashboardSkeleton,
  DashboardSummary,
  EmptyDashboard,
  SystemHealth,
  UsersByRole,
} from "../components/dashboard";

import { useAdminDashboard } from "../hooks/dashboard/useAdminDashboard";
import { useSystemHealth } from "../hooks/dashboard/useSystemHealth";

export default function AdminDashboardPage() {
  const {
    dashboard,
    loading: dashboardLoading,
    error: dashboardError,
    refetch: refetchDashboard,
  } = useAdminDashboard();

  const {
    health,
    loading: healthLoading,
  } = useSystemHealth();

  if (dashboardLoading) {
    return (
      <div className="space-y-6">
        <DashboardSkeleton />
      </div>
    );
  }

  if (dashboardError || !dashboard) {
    return (
      <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
        <DashboardHeader
          name="Admin"
          description="Monitor users, platform activity, and overall system health."
          icon={ShieldCheck}
        />
        <EmptyDashboard
          message={dashboardError ?? "Unable to load the admin dashboard."}
          onRetry={refetchDashboard}
        />
      </div>
    );
  }

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      {/* Header */}
      <DashboardHeader
        name="Admin"
        description="Monitor users, platform activity, and overall system health."
        icon={ShieldCheck}
      />

      {/* User Summary */}
      <DashboardSummary
        totalUsers={dashboard.totalUsers}
        activeUsers={dashboard.activeUsers}
        inactiveUsers={dashboard.inactiveUsers}
        suspendedUsers={dashboard.suspendedUsers}
      />

      {/* Role Distribution + System Health */}
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-2">
        <UsersByRole roles={dashboard.usersByRole} />
        <SystemHealth health={health} loading={healthLoading} />
      </div>
    </div>
  );
}