import {
  CheckCircle2,
  AlertTriangle,
  XCircle,
  Activity,
} from "lucide-react";

import type {
  HealthCheckEntry,
  SystemHealth as SystemHealthType,
} from "../../types/health";

type SystemHealthProps = {
  health: SystemHealthType | null;
  loading?: boolean;
};

function getStatusIcon(status: HealthCheckEntry["status"]) {
  switch (status) {
    case "Healthy":
      return <CheckCircle2 className="h-5 w-5 text-emerald-600" />;
    case "Degraded":
      return <AlertTriangle className="h-5 w-5 text-amber-500" />;
    case "Unhealthy":
      return <XCircle className="h-5 w-5 text-red-600" />;
  }
}

function getStatusColor(status: HealthCheckEntry["status"]) {
  switch (status) {
    case "Healthy":
      return "text-emerald-700 bg-emerald-50 border-emerald-200";
    case "Degraded":
      return "text-amber-800 bg-amber-50 border-amber-200";
    case "Unhealthy":
      return "text-red-700 bg-red-50 border-red-200";
  }
}

export default function SystemHealth({
  health,
  loading = false,
}: SystemHealthProps) {
  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
        <div className="flex items-center gap-3">
          <div className="h-10 w-10 animate-pulse rounded-full bg-[#EEF3F0]" />
          <div className="h-6 w-40 animate-pulse rounded bg-[#EEF3F0]" />
        </div>

        <div className="mt-8 space-y-4">
          {[1, 2, 3].map((item) => (
            <div
              key={item}
              className="flex items-center justify-between rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-4"
            >
              <div className="flex items-center gap-3">
                <div className="h-5 w-5 animate-pulse rounded-full bg-[#E5EAE7]" />
                <div className="h-4 w-28 animate-pulse rounded bg-[#E5EAE7]" />
              </div>
              <div className="h-6 w-20 animate-pulse rounded-full bg-[#E5EAE7]" />
            </div>
          ))}
        </div>
      </div>
    );
  }

  if (!health) {
    return (
      <div className="rounded-2xl border border-red-200 bg-red-50 p-8 shadow-sm">
        <div className="flex items-center gap-3">
          <div className="flex h-10 w-10 items-center justify-center rounded-full bg-red-100 text-red-600">
            <XCircle size={20} />
          </div>
          <div>
            <h2 className="text-lg font-bold text-red-800">
              System Health Unavailable
            </h2>
            <p className="mt-0.5 text-sm font-medium text-red-600">
              Unable to retrieve current system health status.
            </p>
          </div>
        </div>
      </div>
    );
  }

  const entries = Object.entries(health.entries);

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      {/* Header */}
      <div className="flex items-center justify-between border-b border-[#E5EAE7] p-5 sm:px-8 sm:py-6">
        <div>
          <h2 className="text-lg font-bold text-[#212529]">
            System Health
          </h2>
          <p className="mt-0.5 text-xs font-medium text-[#75837D]">
            Current operational status of application services.
          </p>
        </div>

        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
          <Activity size={20} />
        </div>
      </div>

      {/* Content */}
      <div className="space-y-3 p-5 sm:p-6">
        {entries.map(([name, entry]) => (
          <div
            key={name}
            className="flex items-center justify-between rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] px-5 py-4 transition-colors hover:border-[#315343]"
          >
            <div className="flex items-start gap-4">
              <div className="mt-0.5 shrink-0">
                {getStatusIcon(entry.status)}
              </div>

              <div>
                <p className="text-sm font-bold capitalize text-[#212529]">
                  {name}
                </p>
                {entry.description && (
                  <p className="mt-1 text-xs font-medium text-[#75837D]">
                    {entry.description}
                  </p>
                )}
              </div>
            </div>

            <span
              className={`inline-flex shrink-0 items-center rounded-md border px-2.5 py-1 text-[10px] font-bold uppercase tracking-wider ${getStatusColor(
                entry.status
              )}`}
            >
              {entry.status}
            </span>
          </div>
        ))}
      </div>
    </div>
  );
}