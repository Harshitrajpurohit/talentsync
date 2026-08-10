import {
  Briefcase,
  FileText,
  ClipboardCheck,
  CalendarDays,
  CheckCircle2,
  Clock3,
} from "lucide-react";

import { StatCard } from "../../../../shared/components/StatCard";

import type { RecruiterDashboard } from "../../types/dashboard";

interface RecruiterStatsGridProps {
  dashboard: RecruiterDashboard;
}

export default function RecruiterStatsGrid({
  dashboard,
}: RecruiterStatsGridProps) {
  return (
    <div className="grid gap-5 sm:grid-cols-2 xl:grid-cols-3">
      <StatCard
        title="Open Jobs"
        value={dashboard.openJobs}
        icon={Briefcase}
      />

      <StatCard
        title="Applications"
        value={dashboard.totalApplications}
        icon={FileText}
      />

      <StatCard
        title="Pending Screenings"
        value={dashboard.pendingScreenings}
        icon={ClipboardCheck}
      />

      <StatCard
        title="Applications Today"
        value={dashboard.applicationsToday}
        icon={CalendarDays}
      />

      <StatCard
        title="Interviews Scheduled"
        value={dashboard.interviewsScheduled}
        icon={Clock3}
      />

      <StatCard
        title="Closed Jobs"
        value={dashboard.closedJobs}
        icon={CheckCircle2}
      />
    </div>
  );
}