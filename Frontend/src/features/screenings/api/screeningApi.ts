import { api } from "../../../shared";

import type {
  CreateScreeningRequest,
  ScreeningResponse,
} from "../types/screening";

export const screeningApi = {
  createScreening: (
    request: CreateScreeningRequest,
  ) =>
    api
      .post<ScreeningResponse>(
        "/screenings",
        request,
      )
      .then((response) => response.data),
};