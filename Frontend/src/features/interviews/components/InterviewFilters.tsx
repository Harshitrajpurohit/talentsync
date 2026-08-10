import { Search, CalendarDays, X } from "lucide-react";
import type { InterviewStatus } from "../../../shared/types/recruitment";

interface InterviewFiltersProps {
  search: string;
  status: InterviewStatus | undefined;
  fromDate: string;
  toDate: string;
  onSearchChange: (value: string) => void;
  onStatusChange: (value: InterviewStatus | undefined) => void;
  onFromDateChange: (value: string) => void;
  onToDateChange: (value: string) => void;
  onClear: () => void;
}

export default function InterviewFilters({
  search,
  status,
  fromDate,
  toDate,
  onSearchChange,
  onStatusChange,
  onFromDateChange,
  onToDateChange,
  onClear,
}: InterviewFiltersProps) {
  const hasFilters = Boolean(search.trim()) || Boolean(status) || Boolean(fromDate) || Boolean(toDate);

  // Shared styles for a clean, premium input look
  const inputClass =
    "w-full rounded-[10px] border border-[#E5EAE7] bg-[#F8FAF9] py-2.5 text-sm font-medium text-[#212529] outline-none transition-all placeholder:text-[#A0AAA5] focus:border-[#315343] focus:bg-white focus:ring-1 focus:ring-[#315343]";
  const labelClass =
    "mb-1.5 block text-xs font-bold uppercase tracking-wider text-[#75837D]";

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-5 shadow-sm">
      <div className="grid grid-cols-1 items-end gap-4 sm:grid-cols-2 lg:grid-cols-[2fr_1.5fr_1fr_1fr_auto]">
        
        {/* Search */}
        <div>
          <label className={labelClass}>Search</label>
          <div className="relative">
            <Search
              size={16}
              className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]"
            />
            <input
              type="text"
              value={search}
              onChange={(e) => onSearchChange(e.target.value)}
              placeholder="Candidate or job..."
              className={`${inputClass} pl-10 pr-4`}
            />
          </div>
        </div>

        {/* Status */}
        <div>
          <label className={labelClass}>Status</label>
          <div className="relative">
            <select
              value={status ?? ""}
              onChange={(e) => {
                const val = e.target.value;
                onStatusChange(val === "" ? undefined : (val as InterviewStatus));
              }}
              className={`${inputClass} appearance-none cursor-pointer pl-4 pr-10`}
            >
              <option value="">All Statuses</option>
              <option value="Scheduled">Scheduled</option>
              <option value="Completed">Completed</option>
              <option value="Passed">Passed</option>
              <option value="Failed">Failed</option>
              <option value="Cancelled">Cancelled</option>
            </select>
            <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="16"
                height="16"
                viewBox="0 0 24 24"
                fill="none"
                stroke="currentColor"
                strokeWidth="2"
                strokeLinecap="round"
                strokeLinejoin="round"
              >
                <path d="m6 9 6 6 6-6" />
              </svg>
            </div>
          </div>
        </div>

        {/* From Date */}
        <div>
          <label className={labelClass}>From</label>
          <div className="relative">
            <CalendarDays
              size={16}
              className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]"
            />
            <input
              type="date"
              value={fromDate}
              onChange={(e) => onFromDateChange(e.target.value)}
              className={`${inputClass} pl-10 pr-4`}
            />
          </div>
        </div>

        {/* To Date */}
        <div>
          <label className={labelClass}>To</label>
          <div className="relative">
            <CalendarDays
              size={16}
              className="absolute left-3.5 top-1/2 -translate-y-1/2 text-[#75837D]"
            />
            <input
              type="date"
              value={toDate}
              onChange={(e) => onToDateChange(e.target.value)}
              className={`${inputClass} pl-10 pr-4`}
            />
          </div>
        </div>

        {/* Clear Button */}
        <button
          type="button"
          onClick={onClear}
          disabled={!hasFilters}
          className="inline-flex h-[42px] items-center justify-center gap-2 rounded-[10px] border border-[#E5EAE7] bg-white px-5 text-sm font-bold text-[#75837D] transition-all hover:bg-[#EEF3F0] hover:text-[#212529] active:scale-95 disabled:pointer-events-none disabled:opacity-40"
        >
          <X size={16} />
          Clear
        </button>

      </div>
    </div>
  );
}