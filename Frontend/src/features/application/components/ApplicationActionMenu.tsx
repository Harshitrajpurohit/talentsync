import { getAuth } from "../../../shared/api/authStorage";
import type { ApplicationStatus } from "../../../shared/types/recruitment";

interface ApplicationActionMenuProps {
  status: ApplicationStatus;

  onView: () => void;
  onScreening: () => void;
  onInterview: () => void;
  onSelection: () => void;
}

export default function ApplicationActionMenu({
  status,
  onView,
  onScreening,
  onInterview,
  onSelection,
}: ApplicationActionMenuProps) {
  const role = getAuth()?.role;

  const canScreen =
    role === "Recruiter" && status === "Submitted";

  const canScheduleInterview =
    role === "HR" && status === "Screening";

  const canMakeDecision =
    role === "HR" && status === "InterviewCompleted";

  return (
    <div className="flex justify-end gap-2">
      <button
        onClick={onView}
        className="rounded-full border border-[#E5EAE7] bg-white px-4 py-1.5 text-xs font-bold text-[#75837D] shadow-sm transition-all hover:bg-[#EEF3F0] hover:text-[#212529] active:scale-95"
      >
        View
      </button>

      {canScreen && (
        <button
          onClick={onScreening}
          className="rounded-full bg-[#315343] px-4 py-1.5 text-xs font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
        >
          Screening
        </button>
      )}

      {canScheduleInterview && (
        <button
          onClick={onInterview}
          className="rounded-full bg-[#315343] px-4 py-1.5 text-xs font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
        >
          Schedule Interview
        </button>
      )}

      {canMakeDecision && (
        <button
          onClick={onSelection}
          className="rounded-full bg-[#315343] px-4 py-1.5 text-xs font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
        >
          Final Decision
        </button>
      )}
    </div>
  );
}