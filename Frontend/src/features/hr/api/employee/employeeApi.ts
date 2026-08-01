import api from "../../../../shared/api/axios";

import type {
  PaginationRequest,
  PaginationResponse,
} from "../../../../shared/types/pagination";

import type { Employee } from "../../types/employee";

export async function getEmployees(
  pagination: PaginationRequest,
): Promise<PaginationResponse<Employee>> {
  const response = await api.get<PaginationResponse<Employee>>(
    "/employees",
    {
      params: pagination,
    },
  );

  return response.data;
}