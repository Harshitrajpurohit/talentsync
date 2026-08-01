import EmployeeTableRow from "./EmployeeTableRow";
import type { Employee } from "../../types/employee";

interface EmployeeTableProps {
  employees: Employee[];
}

export default function EmployeeTable({ employees }: EmployeeTableProps) {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Employee</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Code</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Department</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Position</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Join Date</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Status</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7]">
            {employees.map((employee) => (
              <EmployeeTableRow key={employee.id} employee={employee} />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}