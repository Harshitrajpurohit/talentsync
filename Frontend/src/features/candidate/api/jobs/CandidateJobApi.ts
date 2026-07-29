import { api } from "../../../../shared/";

import type {
  CandidateJob,
  CandidateJobDetails,
} from "../../types/job";
import type { PaginationResponse } from "../../../../shared/types/pagination";

export const candidateJobApi = {
  getJobs: (pageNumber = 1, pageSize = 10) =>
    api
      .get<PaginationResponse<CandidateJob>>("/jobs/candidate", {
        params: {
          pageNumber,
          pageSize,
        },
      })
      .then((r) => r.data),

  getJobById: (id: string) =>
    api
      .get<CandidateJobDetails>(`/jobs/candidate/${id}`)
      .then((r) => r.data),
};