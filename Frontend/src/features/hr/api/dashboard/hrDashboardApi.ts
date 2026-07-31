import { api } from "../../../../shared/api/axios";

import type { HrDashboard } from "../../types/dashboard";

export const hrDashboardApi = {
  getDashboard: async (): Promise<HrDashboard> => {
    const response = await api.get<HrDashboard>("/dashboard");
    return response.data;
  },
};