import { X } from "lucide-react";
import JobForm from "../JobForm";
import { useUpdateJob } from "../../../hooks/job/useUpdateJob";
import type { JobDetails, UpdateJobRequest } from "../../../types/job";

interface EditJobDialogProps {
  open: boolean;
  job: JobDetails | null;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function EditJobDialog({ open, job, onClose, onSuccess }: EditJobDialogProps) {
  const { update, loading, error } = useUpdateJob();
  if (!open || !job) return null;

  async function handleSubmit(values: UpdateJobRequest) {
    const updated = await update(job!.id, values);
    if (updated) {
      onSuccess?.();
      onClose();
    }
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all dark:bg-black/60">
      <div className="flex max-h-[90vh] w-full max-w-2xl flex-col overflow-hidden rounded-2xl bg-[#EEF3F0] shadow-xl dark:bg-[#1E3329]">
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] bg-white px-6 py-4 dark:border-[#315343] dark:bg-[#253f33]">
          <div>
            <h2 className="text-xl font-bold text-[#212529] dark:text-white">Edit Job</h2>
            <p className="text-xs font-medium text-[#75837D] dark:text-white/70">Update details for this position.</p>
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
          <JobForm initialValues={job} submitText="Save Changes" loading={loading} onSubmit={handleSubmit} />
        </div>
      </div>
    </div>
  );
}