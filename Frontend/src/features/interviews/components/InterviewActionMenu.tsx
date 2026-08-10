import type { InterviewStatus } from "../../../shared/types/recruitment";

interface InterviewActionMenuProps {
  status: InterviewStatus;
  onRecordOutcome: () => void;
}

export default function InterviewActionMenu({
  status,
  onRecordOutcome,
}: InterviewActionMenuProps) {
  if (status !== "Scheduled") {
    return (
      <span className="text-xs font-bold uppercase tracking-wider text-[#A0AAA5]">
        No Actions
      </span>
    );
  }

  return (
    <button
      type="button"
      onClick={onRecordOutcome}
      className="inline-flex items-center justify-center rounded-full bg-[#315343] px-4 py-1.5 text-xs font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
    >
      Record Outcome
    </button>
  );
}