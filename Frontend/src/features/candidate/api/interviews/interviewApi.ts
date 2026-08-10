import api from "../../../../shared/api/axios";

import type { PaginationResponse , InterviewPaginationRequest} from "../../../../shared/types/pagination";

import type {
  CandidateInterview,
} from "../../types/interview";

export const interviewApi = {
  async getMyInterviews(
    pagination: InterviewPaginationRequest,
  ): Promise<PaginationResponse<CandidateInterview>> {
    const { data } = await api.get<
      PaginationResponse<CandidateInterview>
    >("/interviews/candidate", {
      params: pagination,
    });

    return data;
  },
};