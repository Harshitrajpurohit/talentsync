import { useMemo, useState } from "react";

import SearchBar from "../../../shared/components/SearchBar";
import Pagination from "../../../shared/components/Pagination";

import EmployeeFilters from "../components/employees/EmployeeFilters";
import EmployeeTable from "../components/employees/EmployeeTable";
import EmployeeEmpty from "../components/employees/EmployeeEmpty";
import EmployeeSkeleton from "../components/employees/EmployeeSkeleton";

import { useEmployees } from "../hooks/employee/useEmployees";
import type { EmployeeStatus } from "../../../shared/types/employee";
import type { Employee } from "../types/employee";

export default function EmployeesPage() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<EmployeeStatus | "All">("All");

  const { employees, loading, error } = useEmployees(page, 10);

  const filteredEmployees = useMemo(() => {
    if (!employees) return [];

    return employees.data.filter((employee: Employee) => {
      const matchesSearch =
        employee.name.toLowerCase().includes(search.toLowerCase()) ||
        employee.email.toLowerCase().includes(search.toLowerCase()) ||
        employee.employeeCode.toLowerCase().includes(search.toLowerCase());

      const matchesStatus = status === "All" || employee.status === status;

      return matchesSearch && matchesStatus;
    });
  }, [employees, search, status]);

  if (loading) {
    return (
      <div className="space-y-6">
        <h1 className="text-2xl font-bold text-[#212529]">Employee Directory</h1>
        <EmployeeSkeleton />
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-sm font-medium text-red-600">
        {error}
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Page Header */}
      <div className="flex flex-col gap-1">
        <h1 className="text-2xl font-bold text-[#212529]">Employee Directory</h1>
        <p className="text-sm text-[#75837D]">
          Manage and view all employee records across departments.
        </p>
      </div>

      {/* Controls / Filters */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between rounded-2xl border border-[#E5EAE7] bg-white p-4 shadow-sm">
        <div className="w-full md:max-w-md">
          <SearchBar value={search} onChange={setSearch} />
        </div>

        <EmployeeFilters status={status} onStatusChange={setStatus} />
      </div>

      {/* Content */}
      {filteredEmployees.length === 0 ? (
        <EmployeeEmpty />
      ) : (
        <div className="space-y-4">
          <EmployeeTable employees={filteredEmployees} />

          <Pagination
            pageNumber={employees?.pageNumber ?? 1}
            pageSize={employees?.pageSize ?? 10}
            totalRecords={employees?.totalRecords ?? 0}
            onPageChange={setPage}
          />
        </div>
      )}
    </div>
  );
}