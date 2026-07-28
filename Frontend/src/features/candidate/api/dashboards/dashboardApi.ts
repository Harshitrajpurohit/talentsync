import { api } from "../../../../shared";
import type {
    CandidateDashboard
} from "../../types/dashboard";

export const dashboardApi = {
  getDashboard: () =>
    api.get<CandidateDashboard>("/dashboard").then((r) => r.data),
};