import { RefreshCw } from "lucide-react";

type EmptyDashboardProps = {
  message?: string;
  onRetry?: () => void;
};

export default function EmptyDashboard({
  message = "Unable to load dashboard data.",
  onRetry,
}: EmptyDashboardProps) {
  return (
    <div className="flex min-h-[400px] items-center justify-center rounded-[20px] border border-[#E5EAE7] bg-white dark:border-[#315343] dark:bg-[#253F33]">
      <div className="flex max-w-md flex-col items-center px-6 text-center">
        <div className="flex h-14 w-14 items-center justify-center rounded-full bg-[#F8FAF9] text-[#315343] dark:bg-[#1E3329] dark:text-[#C3F53C]">
          <RefreshCw size={24} />
        </div>

        <h2 className="mt-4 text-lg font-bold text-[#212529] dark:text-white">
          Dashboard Unavailable
        </h2>

        <p className="mt-2 text-sm text-[#75837D] dark:text-white/60">
          {message}
        </p>

        {onRetry && (
          <button
            type="button"
            onClick={onRetry}
            className="mt-5 rounded-full bg-[#315343] px-5 py-2.5 text-sm font-bold text-white transition hover:bg-[#264536] dark:bg-[#C3F53C] dark:text-[#315343]"
          >
            Try Again
          </button>
        )}
      </div>
    </div>
  );
}