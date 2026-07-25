import type { UserRole } from "../../shared/components/sidebar/types";


export interface AuthUser {
  token: string;
  userId: string;
  fullName: string;
  email: string;
  role: UserRole;
}
