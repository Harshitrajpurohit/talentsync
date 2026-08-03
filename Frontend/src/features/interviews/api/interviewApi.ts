import { api } from "../../../shared";

import type {
  InterviewResponse,
  ScheduleInterviewRequest,
} from "../types/interview";

export const interviewApi = {
  scheduleInterview: (
    request: ScheduleInterviewRequest,
  ) =>
    api
      .post<InterviewResponse>(
        "/interviews",
        request,
      )
      .then((response) => response.data),
};