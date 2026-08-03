import {
  Users,
  FileText,
  Search,
  CalendarCheck,
  CheckCircle,
  XCircle,
} from "lucide-react";
import type { JobSummary } from "../../types/job";
import { StatCard } from "../../../../shared/components/StatCard";

interface JobSummaryCardsProps {
  summary: JobSummary;
}

export default function JobSummaryCards({ summary }: JobSummaryCardsProps) {
  const cards = [
    {
      title: "Applications",
      value: summary.totalApplications,
      icon: Users,
    },
    {
      title: "Submitted",
      value: summary.submitted,
      icon: FileText,
    },
    {
      title: "Screening",
      value: summary.screening,
      icon: Search,
    },
    {
      title: "Interview",
      value: summary.interview,
      icon: CalendarCheck,
    },
    {
      title: "Selected",
      value: summary.selected,
      icon: CheckCircle,
    },
    {
      title: "Rejected",
      value: summary.rejected,
      icon: XCircle,
    },
  ];

  return (
    <div className="grid gap-4 sm:grid-cols-2 xl:grid-cols-3">
      {cards.map((card) => {
        const Icon = card.icon;
        return (
          <div key={card.title}>
            <StatCard title={card.title} value={card.value} icon={Icon} />
          </div>
        );
      })}
    </div>
  );
}