import api from "../../../../shared/api/axios";
import type { AdminDashboard } from "../../types/dashboard";

export const adminDashboardApi = {
  async getDashboard(): Promise<AdminDashboard> {
    const response = await api.get<AdminDashboard>("/dashboard");

    return response.data;
  },
};