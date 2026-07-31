export type UserStatus =
  | "Active"
  | "Inactive"
  | "Suspended"
  | "Deleted";

export interface User {
  id: string;
  name: string;
  email: string;
  phone?: string;
  status: UserStatus;
  profilePictureUrl?: string;
  dateOfBirth?: string;
  gender?: string;
  address?: string;
  about?: string;
  linkedinUrl?: string;
  githubUrl?: string;
  portfolioUrl?: string;
  createdAt: string;
}