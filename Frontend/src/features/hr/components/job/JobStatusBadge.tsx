import type { JobStatus } from "../../../../shared/types/jobs";

interface JobStatusBadgeProps {
  status: JobStatus;
}

export default function JobStatusBadge({ status }: JobStatusBadgeProps) {
  const styles =
    status === "Open"
      ? "bg-[#C3F53C]/30 text-[#315343] dark:bg-[#C3F53C]/20 dark:text-[#C3F53C]"
      : "bg-red-100 text-red-700 dark:bg-red-900/40 dark:text-red-300";

  return (
    <span
      className={`inline-flex rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${styles}`}
    >
      {status}
    </span>
  );
}