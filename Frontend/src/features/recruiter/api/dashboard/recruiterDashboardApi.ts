import { api } from "../../../../shared/api/axios";

import type { RecruiterDashboard } from "../../types/dashboard";

export const recruiterDashboardApi = {
  getDashboard: () =>
    api
      .get<RecruiterDashboard>("/dashboard")
      .then((response) => response.data),
};