import { Search } from "lucide-react";
import type { ApplicationStatus } from "../../../shared/types/recruitment";

interface ApplicationFiltersProps {
  search: string;
  status: string;
  onSearchChange: (value: string) => void;
  onStatusChange: (value: string) => void;
}

const statuses: ApplicationStatus[] = [
  "Submitted",
  "Screening",
  "InterviewScheduled",
  "InterviewCompleted",
  "Selected",
  "Rejected",
];

export default function ApplicationFilters({
  search,
  status,
  onSearchChange,
  onStatusChange,
}: ApplicationFiltersProps) {
  return (
    <div className="flex flex-col gap-4 rounded-2xl border border-[#E5EAE7] bg-white p-4 shadow-sm lg:flex-row lg:items-center">
      <div className="relative flex-1">
        <Search
          size={18}
          className="absolute left-4 top-1/2 -translate-y-1/2 text-[#75837D]"
        />
        <input
          value={search}
          onChange={(e) => onSearchChange(e.target.value)}
          placeholder="Search applications by candidate, email, or job title..."
          className="w-full rounded-full border border-transparent bg-[#EEF3F0]/50 py-2.5 pl-11 pr-4 text-sm font-medium text-[#212529] outline-none transition-all placeholder:text-[#75837D] focus:border-[#315343] focus:bg-white focus:ring-1 focus:ring-[#315343]"
        />
      </div>

      <div className="relative shrink-0">
        <select
          value={status}
          onChange={(e) => onStatusChange(e.target.value)}
          className="appearance-none rounded-full border border-[#E5EAE7] bg-white py-2.5 pl-5 pr-10 text-sm font-medium text-[#212529] shadow-sm outline-none transition-all hover:border-[#315343] focus:border-[#315343] focus:ring-1 focus:ring-[#315343] cursor-pointer"
        >
          <option value="">All Statuses</option>
          {statuses.map((s) => (
            <option key={s} value={s}>
              {s.replace(/([A-Z])/g, " $1").trim()}
            </option>
          ))}
        </select>
        <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
          <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6"/></svg>
        </div>
      </div>
    </div>
  );
}