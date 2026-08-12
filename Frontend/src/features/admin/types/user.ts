import type { UserRole } from "../../../shared/types/role";
import type { UserStatus } from "../../../shared/types/user";


export interface UserWithRole {
  id: string;
  userId: string;
  roleId: string;

  name: string;
  email: string;
  phone?: string;

  status: UserStatus;
  role: UserRole;

  isDeleted: boolean;
  createdAt: string;
}