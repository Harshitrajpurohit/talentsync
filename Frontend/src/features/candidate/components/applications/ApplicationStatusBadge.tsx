import type { ApplicationStatus } from "../../../../shared/types/recruitment";

interface ApplicationStatusBadgeProps {
  status: ApplicationStatus;
}

function getStatusStyles(status: ApplicationStatus) {
  switch (status) {
    case "Submitted":
      return "bg-[#E5EAE7]/50 text-[#75837D] dark:bg-gray-800 dark:text-gray-300";
    case "Screening":
      return "bg-yellow-100 text-yellow-700 dark:bg-yellow-900/30 dark:text-yellow-400";
    case "InterviewScheduled":
      return "bg-blue-100 text-blue-700 dark:bg-blue-900/30 dark:text-blue-400";
    case "InterviewCompleted":
      return "bg-indigo-100 text-indigo-700 dark:bg-indigo-900/30 dark:text-indigo-400";
    case "Selected":
      return "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400";
    case "Rejected":
      return "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400";
    default:
      return "bg-[#E5EAE7]/50 text-[#75837D] dark:bg-gray-800 dark:text-gray-300";
  }
}

export default function ApplicationStatusBadge({
  status,
}: ApplicationStatusBadgeProps) {
  return (
    <span
      className={`inline-flex rounded-full px-3 py-1 text-xs font-medium ${getStatusStyles(
        status
      )}`}
    >
      {status}
    </span>
  );
}