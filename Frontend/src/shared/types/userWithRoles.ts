import type { UserRole } from "./role";
import type { UserStatus } from "./user";

export interface UserWithRoles {
  id: string;
  name: string;
  email: string;
  phone?: string;
  status: UserStatus;
  roles: UserRole[];
}