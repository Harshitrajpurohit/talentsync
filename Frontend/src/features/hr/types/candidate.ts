import type { UserStatus } from "../../../shared/types/user";
import type { User } from "../../../shared/types/user";
import type { ApplicationWithDetails } from "../../application/types/application";
import type { InterviewDetailed } from "../../interviews/types/interview";


export interface CandidateListItem {
  id: string;

  name: string;

  email: string;

  phone?: string;

  profilePictureUrl?: string;

  status: UserStatus;

  createdAt: string;
}


export interface CandidateDetails {
  profile: User;

  applications: ApplicationWithDetails[];

  interviews: InterviewDetailed[];
}