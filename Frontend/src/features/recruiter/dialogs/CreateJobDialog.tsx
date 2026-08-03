import { X } from "lucide-react";
import JobForm from "../../jobs/components/JobForm";
import { useCreateJob } from "../hooks/useCreateJob";
import type { CreateJobRequest } from "../../jobs/types/job";

interface CreateJobDialogProps {
  open: boolean;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function CreateJobDialog({ open, onClose, onSuccess }: CreateJobDialogProps) {
  const { create, loading, error } = useCreateJob();
  if (!open) return null;

  async function handleSubmit(values: CreateJobRequest) {
    const created = await create(values);
    if (created) {
      onSuccess?.();
      onClose();
    }
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all dark:bg-black/60">
      <div className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-2xl bg-[#EEF3F0] shadow-xl dark:bg-[#1E3329]">
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] bg-white px-6 py-4 dark:border-[#315343] dark:bg-[#253f33]">
          <div>
            <h2 className="text-xl font-bold text-[#212529] dark:text-white">Create Job</h2>
            <p className="text-xs font-medium text-[#75837D] dark:text-white/70">Create a new job opening.</p>
          </div>
          <button
            type="button"
            onClick={onClose}
            className="rounded-lg p-2 text-[#75837D] transition-colors hover:bg-[#EEF3F0] hover:text-[#315343] dark:hover:bg-[#315343] dark:hover:text-white"
          >
            <X size={20} />
          </button>
        </div>
        <div className="overflow-y-auto px-6 py-5">
          {error && <div className="mb-4 rounded-xl border border-red-200 bg-red-50 p-3 text-sm font-medium text-red-600 dark:border-red-900/50 dark:bg-red-900/20 dark:text-red-400">{error}</div>}
          <JobForm submitText="Create Job" loading={loading} onSubmit={handleSubmit} />
        </div>
      </div>
    </div>
  );
}