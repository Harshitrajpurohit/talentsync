import { ArrowLeft, Pencil, RefreshCw } from "lucide-react";
import { getAuth } from "../../../../shared/api/authStorage";
import type { JobDetails } from "../../types/job";

interface JobDetailsActionsProps {
  job: JobDetails & { hrId?: string };
  onBack: () => void;
  onEdit: () => void;
  onStatus: () => void;
}

export default function JobDetailsActions({
  job,
  onBack,
  onEdit,
  onStatus,
}: JobDetailsActionsProps) {
  const currentUser = getAuth();
  const role = currentUser?.role;
  const isCreator = currentUser?.userId === job.hrId && role === "Recruiter";

  return (
    <div className="flex flex-wrap justify-end gap-3">
      <button
        type="button"
        onClick={onBack}
        className="inline-flex items-center gap-2 rounded-full border border-[#E5EAE7] bg-white px-5 py-2 text-sm font-bold text-[#75837D] shadow-sm transition-colors hover:bg-[#EEF3F0] hover:text-[#212529] active:scale-95 dark:border-[#315343] dark:bg-[#1E3329] dark:text-white/80 dark:hover:border-[#C3F53C] dark:hover:text-white"
      >
        <ArrowLeft size={16} />
        Back
      </button>

      {isCreator && (
        <button
          type="button"
          onClick={onEdit}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 dark:bg-[#1E3329] dark:text-[#C3F53C] dark:hover:bg-[#C3F53C] dark:hover:text-[#1E3329]"
        >
          <Pencil size={16} />
          Edit Job
        </button>
      )}

      {isCreator && (
        <button
          type="button"
          onClick={onStatus}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 dark:bg-[#1E3329] dark:text-[#C3F53C] dark:hover:bg-[#C3F53C] dark:hover:text-[#1E3329]"
        >
          <RefreshCw size={16} />
          Update Status
        </button>
      )}
    </div>
  );
}