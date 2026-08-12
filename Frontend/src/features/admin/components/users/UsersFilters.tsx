import { Search, X } from "lucide-react";

import type { UserRole } from "../../../../shared/types/role";
import type { UserStatus } from "../../../../shared/types/user";

interface UsersFiltersProps {
  search: string;
  role?: UserRole;
  status?: UserStatus;
  onSearchChange: (value: string) => void;
  onRoleChange: (value: UserRole | undefined) => void;
  onStatusChange: (value: UserStatus | undefined) => void;
}

const roles: UserRole[] = ["Admin", "HR", "Recruiter", "Manager", "Employee", "Candidate"];
const statuses: UserStatus[] = ["Active", "Inactive", "Suspended", "Deleted"];

export default function UsersFilters({
  search,
  role,
  status,
  onSearchChange,
  onRoleChange,
  onStatusChange,
}: UsersFiltersProps) {
  const hasFilters = search.trim().length > 0 || role !== undefined || status !== undefined;

  const clearFilters = () => {
    onSearchChange("");
    onRoleChange(undefined);
    onStatusChange(undefined);
  };

  // Shared styles for a clean, premium input look
  const inputClass =
    "w-full rounded-[10px] border border-[#E5EAE7] bg-[#F8FAF9] py-2.5 text-sm font-medium text-[#212529] outline-none transition-all placeholder:text-[#A0AAA5] focus:border-[#315343] focus:bg-white focus:ring-1 focus:ring-[#315343]";
  const labelClass =
    "mb-1.5 block text-xs font-bold uppercase tracking-wider text-[#75837D]";

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-5 shadow-sm">
      <div className="grid grid-cols-1 items-end gap-4 sm:grid-cols-2 lg:grid-cols-[2fr_1.5fr_1.5fr_auto]">
        
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
              placeholder="Search by name or email..."
              className={`${inputClass} pl-10 pr-4`}
            />
          </div>
        </div>

        {/* Role */}
        <div>
          <label className={labelClass}>Role</label>
          <div className="relative">
            <select
              value={role ?? ""}
              onChange={(e) => onRoleChange(e.target.value ? (e.target.value as UserRole) : undefined)}
              className={`${inputClass} appearance-none cursor-pointer pl-4 pr-10`}
            >
              <option value="">All Roles</option>
              {roles.map((item) => (
                <option key={item} value={item}>
                  {item}
                </option>
              ))}
            </select>
            <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6"/></svg>
            </div>
          </div>
        </div>

        {/* Status */}
        <div>
          <label className={labelClass}>Status</label>
          <div className="relative">
            <select
              value={status ?? ""}
              onChange={(e) => onStatusChange(e.target.value ? (e.target.value as UserStatus) : undefined)}
              className={`${inputClass} appearance-none cursor-pointer pl-4 pr-10`}
            >
              <option value="">All Statuses</option>
              {statuses.map((item) => (
                <option key={item} value={item}>
                  {item}
                </option>
              ))}
            </select>
            <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
              <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6"/></svg>
            </div>
          </div>
        </div>

        {/* Clear Button */}
        <button
          type="button"
          onClick={clearFilters}
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