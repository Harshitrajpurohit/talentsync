import { api } from "../../../shared";

import type { ManagerDashboard } from "../types/dashboard";

export const managerDashboardApi = {
  getDashboard: (): Promise<ManagerDashboard> =>
    api
      .get("/dashboard")
      .then((response) => response.data),
};