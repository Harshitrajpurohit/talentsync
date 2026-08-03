import { Loader2, X } from "lucide-react";
import { useUpdateJobStatus } from "../hooks/useUpdateJobStatus";
import type { JobDetails } from "../../jobs/types/job";
import type { JobStatus } from "../../jobs/types/job";

interface UpdateJobStatusDialogProps {
  open: boolean;
  job: JobDetails | null;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function UpdateJobStatusDialog({ open, job, onClose, onSuccess }: UpdateJobStatusDialogProps) {
  const { updateStatus, loading, error } = useUpdateJobStatus();
  if (!open || !job) return null;
  const nextStatus: JobStatus = job.status === "Open" ? "Closed" : "Open";

  async function handleUpdate() {
    const updated = await updateStatus(job!.id, nextStatus);
    if (updated) {
      onSuccess?.();
      onClose();
    }
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all dark:bg-black/60">
      <div className="w-full max-w-md rounded-2xl bg-white shadow-xl dark:bg-[#253f33]">
        <div className="flex items-center justify-between border-b border-[#E5EAE7] px-6 py-4 dark:border-[#315343]">
          <div>
            <h2 className="text-lg font-bold text-[#212529] dark:text-white">Update Job Status</h2>
          </div>
          <button
            type="button"
            onClick={onClose}
            disabled={loading}
            className="rounded-lg p-2 text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#315343] disabled:opacity-50 dark:hover:bg-[#315343] dark:hover:text-white"
          >
            <X size={20} />
          </button>
        </div>
        <div className="p-6">
          {error && <div className="mb-4 rounded-xl border border-red-200 bg-red-50 p-3 text-sm font-medium text-red-600 dark:border-red-900/50 dark:bg-red-900/20 dark:text-red-400">{error}</div>}
          <div className="mb-6 rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-5 dark:border-[#315343] dark:bg-[#1E3329]">
            <h3 className="font-bold text-[#212529] dark:text-white">{job.title}</h3>
            <div className="mt-4 flex items-center justify-between">
              <div>
                <p className="text-xs font-medium text-[#75837D] dark:text-white/60">Current</p>
                <p className="text-sm font-bold text-[#212529] dark:text-white">{job.status}</p>
              </div>
              <div className="text-[#E5EAE7] dark:text-[#315343]">→</div>
              <div className="text-right">
                <p className="text-xs font-medium text-[#75837D] dark:text-white/60">New Status</p>
                <p className="text-sm font-bold text-[#315343] dark:text-[#C3F53C]">{nextStatus}</p>
              </div>
            </div>
          </div>
          <div className="flex justify-end gap-3">
            <button
              type="button"
              onClick={onClose}
              disabled={loading}
              className="rounded-[10px] border border-[#E5EAE7] px-5 py-2.5 text-sm font-bold text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#212529] disabled:opacity-50 dark:border-[#315343] dark:text-white/80 dark:hover:bg-[#315343] dark:hover:text-white"
            >
              Cancel
            </button>
            <button
              type="button"
              onClick={handleUpdate}
              disabled={loading}
              className="flex min-w-[120px] items-center justify-center rounded-[10px] bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:opacity-50 dark:bg-[#C3F53C] dark:text-[#1E3329] dark:hover:bg-[#b0df35]"
            >
              {loading ? <Loader2 size={16} className="animate-spin text-current" /> : "Update Status"}
            </button>
          </div>
        </div>
      </div>
    </div>
  );
}