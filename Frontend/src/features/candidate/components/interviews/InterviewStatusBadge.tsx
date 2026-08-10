import type { InterviewStatus } from "../../../../shared/types/recruitment";

interface InterviewStatusBadgeProps {
  status: InterviewStatus;
}

const statusClasses: Record<InterviewStatus, string> = {
  Pending: "bg-amber-100 text-amber-800",
  Scheduled: "bg-blue-100 text-blue-700",
  Completed: "bg-indigo-100 text-indigo-700",
  Passed: "bg-[#C3F53C]/30 text-[#315343]",
  Failed: "bg-red-100 text-red-700",
  Cancelled: "bg-gray-100 text-gray-700",
};

export default function InterviewStatusBadge({ status }: InterviewStatusBadgeProps) {
  return (
    <span
      className={`inline-flex rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${statusClasses[status]}`}
    >
      {status}
    </span>
  );
}