import { useEffect, useState } from "react";

import { getEmployees } from "../../api/employee/employeeApi";

import type { Employee } from "../../types/employee";

import type {
  PaginationResponse,
} from "../../../../shared/types/pagination";

export function useEmployees(
  pageNumber: number,
  pageSize: number,
) {
  const [employees, setEmployees] =
    useState<PaginationResponse<Employee>>();

  const [loading, setLoading] =
    useState(true);

  const [error, setError] =
    useState<string>();

  useEffect(() => {
    async function loadEmployees() {
      try {
        setLoading(true);
        setError(undefined);

        const response = await getEmployees({
          pageNumber,
          pageSize,
        });

        setEmployees(response);
      } catch {
        setError("Failed to load employees.");
      } finally {
        setLoading(false);
      }
    }

    loadEmployees();
  }, [pageNumber, pageSize]);

  return {
    employees,
    loading,
    error,
  };
}