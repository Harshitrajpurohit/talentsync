import {
  Briefcase,
  FileText,
  CalendarDays,
  BadgeCheck,
} from "lucide-react";

import { StatCard } from "./StatCard";

interface StatsGridProps {
  stats: {
    totalApplications: number;
    shortlisted: number;
    interviews: number;
    offers: number;
  };
}

export default function StatsGrid({ stats }: StatsGridProps) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
      <StatCard
        title="Total Applications"
        value={stats.totalApplications}
        icon={FileText}
      />
      <StatCard
        title="Shortlisted"
        value={stats.shortlisted}
        icon={BadgeCheck}
      />
      <StatCard
        title="Interviews"
        value={stats.interviews}
        icon={CalendarDays}
      />
      <StatCard
        title="Offers Received"
        value={stats.offers}
        icon={Briefcase}
      />
    </div>
  );
}