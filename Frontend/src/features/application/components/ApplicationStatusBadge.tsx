import type { ApplicationStatus } from "../../../shared/types/recruitment";

interface ApplicationStatusBadgeProps {
  status: ApplicationStatus;
}

export default function ApplicationStatusBadge({
  status,
}: ApplicationStatusBadgeProps) {
  const styles: Record<ApplicationStatus, string> = {
    Submitted: "bg-blue-100 text-blue-700",
    Screening: "bg-amber-100 text-amber-800",
    InterviewScheduled: "bg-purple-100 text-purple-700",
    InterviewCompleted: "bg-indigo-100 text-indigo-700",
    Selected: "bg-[#C3F53C]/30 text-[#315343]",
    Rejected: "bg-red-100 text-red-700",
  };

  const labels: Record<ApplicationStatus, string> = {
    Submitted: "Submitted",
    Screening: "Screening",
    InterviewScheduled: "Interview Scheduled",
    InterviewCompleted: "Interview Completed",
    Selected: "Selected",
    Rejected: "Rejected",
  };

  return (
    <span
      className={`inline-flex rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${styles[status]}`}
    >
      {labels[status]}
    </span>
  );
}