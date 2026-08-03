import { ArrowLeft, CalendarDays, ClipboardCheck, BadgeCheck } from "lucide-react";
import type { ApplicationProfile } from "../../types/application";
import { getAuth } from "../../../../shared/api/authStorage";

interface ApplicationActionsProps {
  application: ApplicationProfile;
  onBack: () => void;
  onScreening: () => void;
  onInterview: () => void;
  onSelection: () => void;
}

export default function ApplicationActions({
  application,
  onBack,
  onScreening,
  onInterview,
  onSelection,
}: ApplicationActionsProps) {

    const role = getAuth()?.role;
  
    const canScreen = role === "Recruiter" && application.status === "Submitted";
  
    const canScheduleInterview = role === "HR" && application.status === "Screening";
  
    const canMakeDecision = role === "HR" && application.status === "InterviewCompleted";

  return (
    <div className="flex flex-wrap justify-end gap-3">
      <button
        onClick={onBack}
        className="inline-flex items-center gap-2 rounded-full border border-[#E5EAE7] bg-white px-5 py-2 text-sm font-bold text-[#75837D] shadow-sm transition-colors hover:bg-[#EEF3F0] hover:text-[#212529] active:scale-95"
      >
        <ArrowLeft size={16} />
        Back
      </button>

      {canScreen  && (
        <button
          onClick={onScreening}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
        >
          <ClipboardCheck size={16} />
          Start Screening
        </button>
      )}

      {canScheduleInterview && (
        <button
          onClick={onInterview}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
        >
          <CalendarDays size={16} />
          Schedule Interview
        </button>
      )}

      {canMakeDecision && (
        <button
          onClick={onSelection}
          className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
        >
          <BadgeCheck size={16} />
          Final Decision
        </button>
      )}
    </div>
  );
}