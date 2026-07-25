// Login

import type { UserRole } from "../../../shared/components/sidebar/types";


export interface LoginRequest {
  email: string;
  password: string;
}

export interface LoginResponse {
  userId: string;
  name: string;
  email: string;
  role: UserRole;
  token: string;
}

// Register

export interface RegisterRequest {
  name: string;
  email: string;
  password: string;
  phone: string;
}

type userStatus = "Active" | "Inactive" | "Suspended"| "Deleted";

export interface RegisterResponse {
  id: string;
  name: string;
  email: string;
  status : userStatus;
  phone : string;
  isDeleted : boolean;
  createdAt : string;
}
