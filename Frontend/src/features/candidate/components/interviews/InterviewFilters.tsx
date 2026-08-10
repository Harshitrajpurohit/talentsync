import type { InterviewStatus } from "../../../../shared/types/recruitment";

interface InterviewFiltersProps {
  search: string;
  status: InterviewStatus | "";
  fromDate: string;
  toDate: string;

  onSearchChange: (value: string) => void;
  onStatusChange: (value: InterviewStatus | "") => void;
  onFromDateChange: (value: string) => void;
  onToDateChange: (value: string) => void;
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
}: InterviewFiltersProps) {
  const inputClass =
    "w-full rounded-[10px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm font-medium text-[#212529] outline-none transition-all placeholder:text-[#75837D] focus:border-[#315343] focus:ring-1 focus:ring-[#315343]";

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-5 shadow-sm">
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
        <div>
          <label className="mb-1.5 block text-xs font-bold uppercase tracking-wider text-[#75837D]">Search</label>
          <input
            type="text"
            placeholder="Job title or interviewer..."
            value={search}
            onChange={(e) => onSearchChange(e.target.value)}
            className={inputClass}
          />
        </div>

        <div>
          <label className="mb-1.5 block text-xs font-bold uppercase tracking-wider text-[#75837D]">Status</label>
          <div className="relative">
            <select
              value={status}
              onChange={(e) => onStatusChange(e.target.value as InterviewStatus | "")}
              className={`${inputClass} appearance-none cursor-pointer pr-10`}
            >
              <option value="">All Statuses</option>
              <option value="Pending">Pending</option>
              <option value="Scheduled">Scheduled</option>
              <option value="Completed">Completed</option>
              <option value="Passed">Passed</option>
              <option value="Failed">Failed</option>
              <option value="Cancelled">Cancelled</option>
            </select>
            <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6"/></svg>
            </div>
          </div>
        </div>

        <div>
          <label className="mb-1.5 block text-xs font-bold uppercase tracking-wider text-[#75837D]">From Date</label>
          <input
            type="date"
            value={fromDate}
            onChange={(e) => onFromDateChange(e.target.value)}
            className={inputClass}
          />
        </div>

        <div>
          <label className="mb-1.5 block text-xs font-bold uppercase tracking-wider text-[#75837D]">To Date</label>
          <input
            type="date"
            value={toDate}
            onChange={(e) => onToDateChange(e.target.value)}
            className={inputClass}
          />
        </div>
      </div>
    </div>
  );
}