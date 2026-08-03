import type { ApplicationStatus } from "../../../../shared/types/recruitment";

interface JobApplicationStatusBadgeProps {
  status: ApplicationStatus;
}

const statusStyles: Record<ApplicationStatus, string> = {
  Submitted:
    "bg-blue-100 text-blue-700 dark:bg-blue-900/40 dark:text-blue-300",
  Screening:
    "bg-amber-100 text-amber-800 dark:bg-amber-900/40 dark:text-amber-300",
  InterviewScheduled:
    "bg-purple-100 text-purple-700 dark:bg-purple-900/40 dark:text-purple-300",
  InterviewCompleted:
    "bg-indigo-100 text-indigo-700 dark:bg-indigo-900/40 dark:text-indigo-300",
  Selected:
    "bg-[#C3F53C]/30 text-[#315343] dark:bg-[#C3F53C]/20 dark:text-[#C3F53C]",
  Rejected:
    "bg-red-100 text-red-700 dark:bg-red-900/40 dark:text-red-300",
};

const labels: Record<ApplicationStatus, string> = {
  Submitted: "Submitted",
  Screening: "Screening",
  InterviewScheduled: "Interview Scheduled",
  InterviewCompleted: "Interview Completed",
  Selected: "Selected",
  Rejected: "Rejected",
};

export default function JobApplicationStatusBadge({
  status,
}: JobApplicationStatusBadgeProps) {
  return (
    <span
      className={`inline-flex rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${statusStyles[status]}`}
    >
      {labels[status]}
    </span>
  );
}