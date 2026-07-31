import type { UserStatus } from "../../../../shared/types/user";

interface CandidateFiltersProps {
  status: UserStatus | "All";
  onStatusChange: (status: UserStatus | "All") => void;
}

export default function CandidateFilters({
  status,
  onStatusChange,
}: CandidateFiltersProps) {
  return (
    <select
      value={status}
      onChange={(e) =>
        onStatusChange(e.target.value as UserStatus | "All")
      }
      className="cursor-pointer rounded-full border border-gray-200 bg-white px-5 py-2.5 text-sm font-medium text-gray-900 outline-none transition-all duration-300 focus:border-[#315343] focus:ring-2 focus:ring-[#315343]/10 dark:border-[#315343] dark:bg-[#1e3329] dark:text-white"
    >
      <option value="All">All</option>
      <option value="Active">Active</option>
      <option value="Inactive">Inactive</option>
      <option value="Suspended">Suspended</option>
      <option value="Deleted">Deleted</option>
    </select>
  );
}