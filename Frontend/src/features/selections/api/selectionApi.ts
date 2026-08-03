import { api } from "../../../shared";

import type {
  CreateSelectionDecisionRequest,
  SelectionResponse,
} from "../types/selection";

export const selectionApi = {
  createSelectionDecision: (
    request: CreateSelectionDecisionRequest,
  ) =>
    api
      .post<SelectionResponse>(
        "/selections",
        request,
      )
      .then((response) => response.data),
};