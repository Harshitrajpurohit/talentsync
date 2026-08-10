import {
  BriefcaseBusiness,
  ClipboardList,
  CalendarDays,
  CalendarClock,
  CircleCheck,
} from "lucide-react";

import { StatCard } from "../../../../shared/components/StatCard";

interface DashboardSummaryProps {
  openJobs: number;
  totalApplications: number;
  interviewsToday: number;
  upcomingInterviews: number;
  completedInterviews: number;
}

export default function DashboardSummary({
  openJobs,
  totalApplications,
  interviewsToday,
  upcomingInterviews,
  completedInterviews,
}: DashboardSummaryProps) {
  return (
    <div className="grid grid-cols-2 gap-4 lg:grid-cols-3 xl:grid-cols-5">
      <StatCard
        title="Open Jobs"
        value={openJobs}
        icon={BriefcaseBusiness}
      />

      <StatCard
        title="Applications"
        value={totalApplications}
        icon={ClipboardList}
      />

      <StatCard
        title="Today's Interviews"
        value={interviewsToday}
        icon={CalendarDays}
      />

      <StatCard
        title="Upcoming Interviews"
        value={upcomingInterviews}
        icon={CalendarClock}
      />

      <StatCard
        title="Completed"
        value={completedInterviews}
        icon={CircleCheck}
      />
    </div>
  );
}