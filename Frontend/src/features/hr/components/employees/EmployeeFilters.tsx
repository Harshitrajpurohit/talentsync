import type { EmployeeStatus } from "../../../../shared/types/employee";

interface EmployeeFiltersProps {
  status: EmployeeStatus | "All";
  onStatusChange: (status: EmployeeStatus | "All") => void;
}

const statuses: (EmployeeStatus | "All")[] = [
  "All",
  "Active",
  "OnLeave",
  "Suspended",
  "Resigned",
  "Terminated",
];

export default function EmployeeFilters({
  status,
  onStatusChange,
}: EmployeeFiltersProps) {
  return (
    <div className="relative">
      <select
        value={status}
        onChange={(e) =>
          onStatusChange(e.target.value as EmployeeStatus | "All")
        }
        className="appearance-none rounded-full border border-[#E5EAE7] bg-white py-2.5 pl-5 pr-10 text-sm font-medium text-[#212529] shadow-sm outline-none transition-all hover:border-[#315343] focus:border-[#315343] focus:ring-1 focus:ring-[#315343] cursor-pointer"
      >
        {statuses.map((value) => (
          <option key={value} value={value}>
            {value === "All" ? "All Statuses" : value}
          </option>
        ))}
      </select>
    </div>
  );
}