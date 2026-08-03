import { Eye, Pencil, RefreshCw } from "lucide-react";
import { getAuth } from "../../../shared/api/authStorage";
import type { JobListItem } from "../types/job";

interface JobActionMenuProps {
  
  job: JobListItem & { hrId?: string }; 
  onView: (job: JobListItem) => void;
  onEdit: (job: JobListItem) => void;
  onStatus: (job: JobListItem) => void;
}

export default function JobActionMenu({
  job,
  onView,
  onEdit,
  onStatus,
}: JobActionMenuProps) {

  const currentUser = getAuth();
  
  const role = currentUser?.role;

  const isCreator = currentUser?.userId === job.hrId && role === "Recruiter";
  
  const buttonBaseClass =
    "flex h-8 w-8 items-center justify-center rounded-full bg-[#315343] text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 dark:bg-[#1E3329] dark:hover:bg-[#C3F53C] dark:hover:text-[#1E3329]";

  return (
    <div className="flex justify-end gap-2">
      <button
        type="button"
        title="View"
        onClick={() => onView(job)}
        className={buttonBaseClass}
      >
        <Eye size={14} strokeWidth={2.5} />
      </button>


      {isCreator && (
        <>
          <button
            type="button"
            title="Edit"
            onClick={() => onEdit(job)}
            className={buttonBaseClass}
          >
            <Pencil size={14} strokeWidth={2.5} />
          </button>

          <button
            type="button"
            title="Update Status"
            onClick={() => onStatus(job)}
            className={buttonBaseClass}
          >
            <RefreshCw size={14} strokeWidth={2.5} />
          </button>
        </>
      )}
    </div>
  );
}