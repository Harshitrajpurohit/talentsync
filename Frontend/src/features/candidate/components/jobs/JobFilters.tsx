import type { JobStatus } from "../../../../shared/types/recruitment";

interface JobFiltersProps {
  value: JobStatus | "All";
  onChange: (value: JobStatus | "All") => void;
}

const statuses: (JobStatus | "All")[] = [
  "All",
  "Open",
  "Closed",
];

export default function JobFilters({
  value,
  onChange,
}: JobFiltersProps) {
  return (
    <select
      value={value}
      onChange={(e) =>
        onChange(e.target.value as JobStatus | "All")
      }
      className="cursor-pointer rounded-[20px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition-all duration-300 hover:border-[#315343] focus:border-[#315343] focus:ring-2 focus:ring-[#C3F53C]/50 dark:border-gray-700 dark:bg-gray-900 dark:text-white dark:focus:ring-[#315343]"
    >
      {statuses.map((status) => (
        <option
          key={status}
          value={status}
        >
          {status}
        </option>
      ))}
    </select>
  );
}