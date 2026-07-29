import type { ApplicationStatus } from "../../../../shared/types/recruitment";

interface ApplicationFiltersProps {
  value: ApplicationStatus | "All";
  onChange: (value: ApplicationStatus | "All") => void;
}

const statuses: (ApplicationStatus | "All")[] = [
  "All",
  "Submitted",
  "Screening",
  "InterviewScheduled",
  "InterviewCompleted",
  "Selected",
  "Rejected",
];

export default function ApplicationFilters({
  value,
  onChange,
}: ApplicationFiltersProps) {
  return (
    <select
      value={value}
      onChange={(e) =>
        onChange(e.target.value as ApplicationStatus | "All")
      }
      className="cursor-pointer rounded-[20px] border border-[#E5EAE7] bg-white px-4 py-2 text-sm text-[#212529] outline-none transition-all duration-300 hover:border-[#315343] focus:border-[#315343] focus:ring-2 focus:ring-[#C3F53C]/50 dark:border-gray-700 dark:bg-gray-900 dark:text-white dark:focus:ring-[#315343]"
    >
      {statuses.map((status) => (
        <option key={status} value={status}>
          {status === "All" ? "All Statuses" : status}
        </option>
      ))}
    </select>
  );
}