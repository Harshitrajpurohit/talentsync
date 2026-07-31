import api from "../../../../shared/api//axios";

import type { PaginationRequest } from "../../../../shared/types/pagination";
import type { PaginationResponse } from "../../../../shared/types/pagination";

import type { User } from "../../../../shared/types/user";

import type { ApplicationWithDetails } from "../../../application/types/application";
import type { InterviewDetailed } from "../../../interviews/types/interview";
import type { Resume } from "../../types/resume";



export async function getCandidates(
  pagination: PaginationRequest
): Promise<PaginationResponse<User>> {
  const response = await api.get<PaginationResponse<User>>("/users/candidates", {
    params: pagination,
  });

  return response.data;
}


export async function getCandidate(
  candidateId: string,
): Promise<User> {
  const response = await api.get<User>(
    `/users/${candidateId}`,
  );

  return response.data;
}



export async function getCandidateApplications(
  candidateId: string,
  pagination: PaginationRequest,
): Promise<PaginationResponse<ApplicationWithDetails>> {
  const response = await api.get<
    PaginationResponse<ApplicationWithDetails>
  >(`/applications/candidate/${candidateId}`, {
    params: pagination,
  });

  return response.data;
}



export async function getCandidateInterviews(
  candidateId: string,
  pagination: PaginationRequest,
): Promise<PaginationResponse<InterviewDetailed>> {
  const response = await api.get<
    PaginationResponse<InterviewDetailed>
  >(`/interviews/candidate/${candidateId}`, {
    params: pagination,
  });

  return response.data;
}



export async function getCandidateResume(
  candidateId: string,
): Promise<Resume> {
  const response = await api.get<Resume>(
    `/resumes/candidate/${candidateId}`,
  );

  return response.data;
}