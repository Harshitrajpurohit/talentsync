import {
  ArrowLeft,
  CalendarDays,
  ClipboardCheck,
  BadgeCheck,
  RotateCcw,
  Ban,
} from "lucide-react";

import type { ApplicationProfile } from "../../types/application";
import { getAuth } from "../../../../shared/api/authStorage";
import type { Interview } from "../../../interviews/types/interview";

interface ApplicationActionsProps {
  application: ApplicationProfile;
  interview : Interview | null;
  onBack: () => void;
  onScreening: () => void;
  onInterview: () => void;
  onSelection: () => void;

  onCancelInterview: () => void;
  onRescheduleInterview: () => void;
}

export default function ApplicationActions({
  application,
  interview,
  onBack,
  onScreening,
  onInterview,
  onSelection,
  onCancelInterview,
  onRescheduleInterview,
}: ApplicationActionsProps) {
  const role = getAuth()?.role;

  const canScreen =
    role === "Recruiter" &&
    application.status === "Submitted";

  const canScheduleInterview =
    role === "HR" &&
    application.status === "Screening";

  const canCancelInterview =
    role === "HR" &&
    application.status === "InterviewScheduled" && 
    interview?.status === "Scheduled";

  const canReScheduleInterview =
    role === "HR" && 
    application.status === "InterviewScheduled" &&
    interview?.status === "Cancelled";

  const canMakeDecision =
    role === "HR" &&
    application.status === "InterviewCompleted";

  return (
    <div className="flex flex-wrap justify-end gap-3">
      <button
        onClick={onBack}
        className="inline-flex items-center gap-2 rounded-full border border-[#E5EAE7] bg-white px-5 py-2 text-sm font-bold text-[#75837D] shadow-sm transition-colors hover:bg-[#EEF3F0] hover:text-[#212529] active:scale-95"
      >
        <ArrowLeft size={16} />
        Back
      </button>

      {canScreen && (
        <button
          onClick={onScreening}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343]"
        >
          <ClipboardCheck size={16} />
          Start Screening
        </button>
      )}

      {canScheduleInterview && (
        <button
          onClick={onInterview}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343]"
        >
          <CalendarDays size={16} />
          Schedule Interview
        </button>
      )}

      {canReScheduleInterview && (
          <button
            onClick={onRescheduleInterview}
            className="inline-flex items-center gap-2 rounded-full bg-amber-500 px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-amber-600"
          >
            <RotateCcw size={16} />
            Reschedule
          </button>

        )}

        {canCancelInterview && (
          <button
            onClick={onCancelInterview}
            className="inline-flex items-center gap-2 rounded-full bg-red-600 px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-red-700"
          >
            <Ban size={16} />
            Cancel Interview
          </button>
        )}

      {canMakeDecision && (
        <button
          onClick={onSelection}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343]"
        >
          <BadgeCheck size={16} />
          Final Decision
        </button>
      )}
    </div>
  );
}