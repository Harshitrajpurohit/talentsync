import type { JobStatus } from "../../../../shared/types/recruitment";

interface JobStatusBadgeProps {
  status: JobStatus;
}

function getStatusStyle(status: JobStatus) {
  switch (status) {
    case "Open":
      return "bg-green-100 text-green-700 dark:bg-green-900/30 dark:text-green-400";
    case "Closed":
      return "bg-red-100 text-red-700 dark:bg-red-900/30 dark:text-red-400";
    default:
      return "bg-[#E5EAE7]/50 text-[#75837D] dark:bg-gray-800 dark:text-gray-300";
  }
}

export default function JobStatusBadge({
  status,
}: JobStatusBadgeProps) {
  return (
    <span
      className={`rounded-full px-3 py-1 text-xs font-medium ${getStatusStyle(
        status
      )}`}
    >
      {status}
    </span>
  );
}