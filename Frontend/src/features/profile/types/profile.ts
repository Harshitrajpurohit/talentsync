import type { User } from "../../../shared/types/user";

export type Profile = User;

export interface UpdateProfileRequest {
  name: string;

  phone?: string;

  dateOfBirth?: string;

  gender?: string;

  address?: string;

  about?: string;

  linkedinUrl?: string;

  githubUrl?: string;

  portfolioUrl?: string;
}