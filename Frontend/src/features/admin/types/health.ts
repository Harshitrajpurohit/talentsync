export type HealthStatus =
  | "Healthy"
  | "Degraded"
  | "Unhealthy";

export interface HealthCheckEntry {
  status: HealthStatus;
  duration: string;
  description?: string;
  data?: Record<string, unknown>;
  tags?: string[];
}

export interface SystemHealth {
  status: HealthStatus;
  totalDuration: string;
  entries: Record<string, HealthCheckEntry>;
}