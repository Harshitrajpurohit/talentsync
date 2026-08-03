import type { JobStatus } from "../../../shared/types/jobs";

interface JobFiltersProps {
  status: JobStatus | "All";
  onStatusChange: (status: JobStatus | "All") => void;
}

export default function JobFilters({
  status,
  onStatusChange,
}: JobFiltersProps) {
  return (
    <div className="relative">
      <select
        value={status}
        onChange={(e) => onStatusChange(e.target.value as JobStatus | "All")}
        className="appearance-none rounded-full border border-[#E5EAE7] bg-white py-2.5 pl-5 pr-10 text-sm font-medium text-[#212529] shadow-sm outline-none transition-all hover:border-[#315343] focus:border-[#315343] focus:ring-1 focus:ring-[#315343] cursor-pointer dark:border-[#315343] dark:bg-[#1E3329] dark:text-white dark:hover:border-[#C3F53C]"
      >
        <option value="All">All Jobs</option>
        <option value="Open">Open</option>
        <option value="Closed">Closed</option>
      </select>
    </div>
  );
}