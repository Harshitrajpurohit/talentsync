import { api } from "../../../../shared";

import type {
  Application,
  ApplicationWithDetails,
  CreateApplicationRequest,
} from "../../../application/types/application";

export const applicationApi = {
  createApplication: (request: CreateApplicationRequest) =>
    api
      .post<Application>("/applications", request)
      .then((r) => r.data),

  getApplications: (
  ) =>
    api
      .get<ApplicationWithDetails[]>(
        "/applications/candidate"
      )
      .then((r) => r.data),

};