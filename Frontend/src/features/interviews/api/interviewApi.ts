import { api } from "../../../shared";
import type { InterviewPaginationRequest, PaginationResponse } from "../../../shared/types/pagination";

import type {
  Interview,
  InterviewResponse,
  ScheduleInterviewRequest,
  UpdateInterviewStatusRequest,
  RescheduleInterviewRequest,
  InterviewDetailed,
} from "../types/interview";

export const interviewApi = {
  scheduleInterview: (
    request: ScheduleInterviewRequest,
  ): Promise<InterviewResponse> =>
    api.post("/interviews", request).then((response) => response.data),

  cancelInterview: (
    interviewId: string,
    request: UpdateInterviewStatusRequest,
  ): Promise<InterviewResponse> =>
    api
      .patch(`/interviews/${interviewId}/cancel`, request)
      .then((response) => response.data),

  rescheduleInterview: (
    interviewId: string,
    request: RescheduleInterviewRequest,
  ): Promise<InterviewResponse> =>
    api
      .patch(`/interviews/${interviewId}/reschedule`, request)
      .then((response) => response.data),

  getInterviewByApplicationId: (
    applicationId: string,
  ): Promise<Interview> =>
    api
      .get(`/interviews/application/${applicationId}`)
      .then((response) => response.data),

  getAssignedInterviews: (
    request: InterviewPaginationRequest
  ): Promise<PaginationResponse<InterviewDetailed>> =>
    api
      .get("/interviews/assigned", { params: request })
      .then((response) => response.data),

  recordOutcome: (
    interviewId: string,
    request: UpdateInterviewStatusRequest,
  ): Promise<InterviewResponse> =>
    api
      .patch(`/interviews/${interviewId}/outcome`, request)
      .then((response) => response.data),
};