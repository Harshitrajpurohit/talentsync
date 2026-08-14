import api from "../../../../shared/api/axios";
import type { SystemHealth } from "../../types/health";

export const healthApi = {
  async getReadiness(): Promise<SystemHealth> {
    const response = await api.get<SystemHealth>("/health");

    return response.data;
  },
};