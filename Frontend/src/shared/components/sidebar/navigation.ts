import {
  Home,
  Users,
  Briefcase,
  ClipboardList,
  CalendarDays,
  User,
  Settings,
  ShieldCheck,
  FileBarChart2,
  UserCheck,
} from "lucide-react";

import type { SidebarItem, UserRole } from "./types";

export const navigation: Record<UserRole, SidebarItem[]> = {
  Admin: [
    {
      title: "Dashboard",
      path: "/admin",
      icon: Home,
    },
    {
      title: "Users",
      path: "/admin/users",
      icon: Users,
    },
    {
      title: "Roles",
      path: "/admin/roles",
      icon: ShieldCheck,
    },
    {
      title: "Jobs",
      path: "/admin/jobs",
      icon: Briefcase,
    },
    {
      title: "Applications",
      path: "/admin/applications",
      icon: ClipboardList,
    },
    {
      title: "Interviews",
      path: "/admin/interviews",
      icon: CalendarDays,
    },
    {
      title: "Reports",
      path: "/admin/reports",
      icon: FileBarChart2,
    },
    {
      title: "Profile",
      path: "/admin/profile",
      icon: User,
    },
    {
      title: "Settings",
      path: "/admin/settings",
      icon: Settings,
    },
  ],

  Recruiter: [
    {
      title: "Dashboard",
      path: "/recruiter",
      icon: Home,
    },
    {
      title: "Jobs",
      path: "/recruiter/jobs",
      icon: Briefcase,
    },
    {
      title: "Candidates",
      path: "/recruiter/candidates",
      icon: Users,
    },
    {
      title: "Applications",
      path: "/recruiter/applications",
      icon: ClipboardList,
    },
    {
      title: "Interviews",
      path: "/recruiter/interviews",
      icon: CalendarDays,
    },
    {
      title: "Profile",
      path: "/recruiter/profile",
      icon: User,
    },
    {
      title: "Settings",
      path: "/recruiter/settings",
      icon: Settings,
    },
  ],

  Candidate: [
    {
      title: "Dashboard",
      path: "/candidate",
      icon: Home,
    },
    {
      title: "Browse Jobs",
      path: "/candidate/jobs",
      icon: Briefcase,
    },
    {
      title: "My Applications",
      path: "/candidate/applications",
      icon: ClipboardList,
    },
    {
      title: "Interviews",
      path: "/candidate/interviews",
      icon: CalendarDays,
    },
    {
      title: "Profile",
      path: "/candidate/profile",
      icon: User,
    },
    {
      title: "Settings",
      path: "/candidate/settings",
      icon: Settings,
    },
  ],

  Employee: [
    {
      title: "Dashboard",
      path: "/employee",
      icon: Home,
    },
    {
      title: "Profile",
      path: "/employee/profile",
      icon: User,
    },
    {
      title: "Settings",
      path: "/employee/settings",
      icon: Settings,
    },
  ],

  HR: [
    {
      title: "Dashboard",
      path: "/hr",
      icon: Home,
    },
    {
      title: "Employees",
      path: "/hr/employees",
      icon: UserCheck,
    },
    {
      title: "Candidates",
      path: "/hr/candidates",
      icon: Users,
    },
    {
      title: "Jobs",
      path: "/hr/jobs",
      icon: Briefcase,
    },
    {
      title: "Applications",
      path: "/hr/applications",
      icon: ClipboardList,
    },
    // {
    //   title: "Reports",
    //   path: "/hr/reports",
    //   icon: FileBarChart2,
    // },
    {
      title: "Profile",
      path: "/hr/profile",
      icon: User,
    },
    {
      title: "Settings",
      path: "/hr/settings",
      icon: Settings,
    },
  ],

  Manager: [
    {
      title: "Dashboard",
      path: "/manager",
      icon: Home,
    },
    {
      title: "Jobs",
      path: "/manager/jobs",
      icon: Briefcase,
    },
    {
      title: "Candidates",
      path: "/manager/candidates",
      icon: Users,
    },
    {
      title: "Applications",
      path: "/manager/applications",
      icon: ClipboardList,
    },
    {
      title: "Interviews",
      path: "/manager/interviews",
      icon: CalendarDays,
    },
    {
      title: "Reports",
      path: "/manager/reports",
      icon: FileBarChart2,
    },
    {
      title: "Profile",
      path: "/manager/profile",
      icon: User,
    },
    {
      title: "Settings",
      path: "/manager/settings",
      icon: Settings,
    },
  ],
};