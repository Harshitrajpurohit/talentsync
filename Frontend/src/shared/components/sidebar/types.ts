import type { LucideIcon } from "lucide-react";

export type UserRole =
  | "Admin"
  | "Recruiter"
  | "Candidate"
  | "Employee"
  | "HR"
  | "Manager"
  | "Employee";

export interface SidebarItem {
  title: string;
  path: string;
  icon: LucideIcon;

  badge?: number;

  children?: SidebarItem[];

  disabled?: boolean;
}