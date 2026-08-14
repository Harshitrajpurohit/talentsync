import type { UserRole } from "../../../shared/types/role";

export interface AdminRoleCount {
  role: UserRole;
  count: number;
}

export interface AdminDashboard {
  totalUsers: number;
  activeUsers: number;
  inactiveUsers: number;
  suspendedUsers: number;
  usersByRole: AdminRoleCount[];
}