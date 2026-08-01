import type { EmployeeStatus } from "../../../shared/types/employee";

export interface Employee {
  id: string;
  employeeCode: string;
  name: string;
  email: string;
  phone?: string;
  profilePictureUrl?: string;
  departmentName: string;
  position: string;
  joinDate: string;
  status: EmployeeStatus;
}