export interface Profile {
  id: string;
  name: string;
  email: string;
  status: string;
  phone?: string;

  profilePictureUrl?: string;

  dateOfBirth?: string;

  gender?: string;

  address?: string;

  about?: string;

  linkedinUrl?: string;

  githubUrl?: string;

  portfolioUrl?: string;

  role: string;
}

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