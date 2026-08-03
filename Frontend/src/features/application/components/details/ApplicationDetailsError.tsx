import { AlertTriangle, RefreshCw } from "lucide-react";

interface ApplicationDetailsErrorProps {
  message?: string;
  onRetry?: () => void;
}

export default function ApplicationDetailsError({
  message = "Failed to load application.",
  onRetry,
}: ApplicationDetailsErrorProps) {
  return (
    <div className="flex min-h-[60vh] items-center justify-center">
      <div className="w-full max-w-lg rounded-2xl border border-red-200 bg-red-50 p-10 text-center shadow-sm">
        <AlertTriangle size={48} className="mx-auto mb-4 text-red-500" />
        
        <h2 className="text-xl font-bold text-red-700">
          Something went wrong
        </h2>
        
        <p className="mt-2 text-sm font-medium text-red-600/80">
          {message}
        </p>

        {onRetry && (
          <button
            onClick={onRetry}
            className="mt-6 inline-flex items-center gap-2 rounded-full bg-red-600 px-6 py-2.5 text-sm font-bold text-white shadow-sm transition hover:bg-red-700 active:scale-95"
          >
            <RefreshCw size={16} />
            Try Again
          </button>
        )}
      </div>
    </div>
  );
}