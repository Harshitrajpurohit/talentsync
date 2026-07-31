import {
  Briefcase,
  FolderOpen,
  Users,
  FileText,
  Calendar,
} from "lucide-react";


import { StatCard } from "../../../../shared/components/StatCard";

type Props = {
  totalJobs: number;
  openJobs: number;
  totalCandidates: number;
  totalApplications: number;
  interviewsToday: number;
};

export default function DashboardSummary({
  totalJobs,
  openJobs,
  totalCandidates,
  totalApplications,
  interviewsToday,
}: Props) {
  return (
    <div className="grid gap-5 md:grid-cols-2 xl:grid-cols-5">
      <StatCard
        title="Total Jobs"
        value={totalJobs}
        icon={Briefcase}
      />
    
      <StatCard
        title="Open Jobs"
        value={openJobs}
        icon={FolderOpen}
      />

      <StatCard
        title="Candidates"
        value={totalCandidates}
        icon={Users}
      />

      <StatCard
        title="Applications"
        value={totalApplications}
        icon={FileText}
      />

      <StatCard
        title="Today's Interviews"
        value={interviewsToday}
        icon={Calendar}
      />
    </div>
  );
}