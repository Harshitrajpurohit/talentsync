import { Link } from "react-router-dom";
import type { Employee } from "../../types/employee";

interface EmployeeTableRowProps {
  employee: Employee;
}

export default function EmployeeTableRow({ employee }: EmployeeTableRowProps) {
  
  // Dynamic color styles based on status
  const getStatusStyles = (status: string) => {
    switch (status) {
      case "Active":
        return "bg-[#C3F53C]/30 text-[#315343]"; // Lime green accent
      case "OnLeave":
        return "bg-amber-100 text-amber-800";
      case "Suspended":
      case "Terminated":
        return "bg-red-100 text-red-700";
      case "Resigned":
        return "bg-gray-100 text-gray-700";
      default:
        return "bg-[#EEF3F0] text-[#75837D]";
    }
  };

  return (
    <tr className="transition-colors hover:bg-[#EEF3F0]/50">
      <td className="px-6 py-4">
        <Link
          to={`/hr/employees/${employee.id}`}
          className="flex items-center gap-3 group"
        >
          <img
            src={
              employee.profilePictureUrl ??
              "https://ui-avatars.com/api/?background=EEF3F0&color=315343&name=" +
                encodeURIComponent(employee.name)
            }
            alt={employee.name}
            className="h-10 w-10 shrink-0 rounded-full object-cover shadow-sm transition-transform group-hover:scale-105"
          />
          <div>
            <p className="font-bold text-[#212529] group-hover:text-[#315343] transition-colors">
              {employee.name}
            </p>
            <p className="text-xs font-medium text-[#75837D]">
              {employee.email}
            </p>
          </div>
        </Link>
      </td>

      <td className="px-6 py-4 font-medium text-[#212529]">
        {employee.employeeCode}
      </td>

      <td className="px-6 py-4 font-medium text-[#75837D]">
        {employee.departmentName}
      </td>

      <td className="px-6 py-4 font-medium text-[#212529]">
        {employee.position}
      </td>

      <td className="px-6 py-4 text-[#75837D]">
        {new Date(employee.joinDate).toLocaleDateString(undefined, {
          year: "numeric",
          month: "short",
          day: "numeric",
        })}
      </td>

      <td className="px-6 py-4">
        <span
          className={`inline-flex items-center rounded-md px-2.5 py-1 text-xs font-bold ${getStatusStyles(
            employee.status
          )}`}
        >
          {employee.status}
        </span>
      </td>
    </tr>
  );
}