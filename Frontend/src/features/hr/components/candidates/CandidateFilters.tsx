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
      className="appearance-none rounded-full border border-[#E5EAE7] bg-white py-2.5 pl-5 pr-10 text-sm font-medium text-[#212529] shadow-sm outline-none transition-all hover:border-[#315343] focus:border-[#315343] focus:ring-1 focus:ring-[#315343] cursor-pointer"
    >
      <option value="All">All</option>
      <option value="Active">Active</option>
      <option value="Inactive">Inactive</option>
      <option value="Suspended">Suspended</option>
      <option value="Deleted">Deleted</option>
    </select>
  );
}