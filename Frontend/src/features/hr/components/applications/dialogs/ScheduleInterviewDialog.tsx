import { useState } from "react";
import { X, Loader2 } from "lucide-react";
import { useScheduleInterview } from "../../../../../features/interviews/hooks/useScheduleInterview";
import type { ScheduleInterviewRequest } from "../../../../../features/interviews/types/interview";
import { useManagers } from "../../../hooks/userrole/useManagers";

interface ScheduleInterviewDialogProps {
  open: boolean;
  applicationId: string;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function ScheduleInterviewDialog({
  open,
  applicationId,
  onClose,
  onSuccess,
}: ScheduleInterviewDialogProps) {
  const { managers } = useManagers();
  const { scheduleInterview, loading, error } = useScheduleInterview();

  const [scheduledAt, setScheduledAt] = useState("");
  const [interviewerId, setInterviewerId] = useState("");
  const [location, setLocation] = useState("");

  if (!open) return null;

  async function handleSubmit() {
    if (!scheduledAt || !interviewerId || !location) return;

    const request: ScheduleInterviewRequest = {
      applicationId,
      scheduledAt,
      interviewerId,
      location,
    };

    const created = await scheduleInterview(request);

    if (created) {
      onSuccess?.();
      setScheduledAt("");
      setInterviewerId("");
      setLocation("");
      onClose();
    }
  }

  const inputClass =
    "w-full rounded-[10px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]";
  const labelClass = "mb-1.5 block text-sm font-semibold text-[#212529]";

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all">
      <div className="flex max-h-[90vh] w-full max-w-lg flex-col overflow-hidden rounded-2xl bg-white shadow-xl">
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] px-6 py-4">
          <div>
            <h2 className="text-lg font-bold text-[#212529]">
              Schedule Interview
            </h2>
            <p className="text-xs font-medium text-[#75837D]">
              Assign an interviewer and set the meeting time.
            </p>
          </div>

          <button
            type="button"
            onClick={onClose}
            className="rounded-lg p-2 text-[#75837D] transition-colors hover:bg-[#EEF3F0] hover:text-[#315343]"
          >
            <X size={20} />
          </button>
        </div>

        <div className="space-y-5 overflow-y-auto p-6">
          {error && (
            <div className="rounded-xl border border-red-200 bg-red-50 p-3 text-sm font-medium text-red-600">
              {error}
            </div>
          )}

          <div>
            <label className={labelClass}>Interviewer</label>
            <select
              value={interviewerId}
              onChange={(e) => setInterviewerId(e.target.value)}
              className={inputClass}
            >
              <option value="" disabled>
                Select Interviewer
              </option>
              {managers?.map((manager) => (
                <option key={manager.userId} value={manager.userId}>
                  {manager.userName}
                </option>
              ))}
            </select>
          </div>

          <div>
            <label className={labelClass}>Date & Time</label>
            <input
              type="datetime-local"
              value={scheduledAt}
              onChange={(e) => setScheduledAt(e.target.value)}
              className={inputClass}
            />
          </div>

          <div>
            <label className={labelClass}>Location / Link</label>
            <input
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              placeholder="e.g. Google Meet / Room 4A"
              className={inputClass}
            />
          </div>
        </div>

        <div className="flex shrink-0 justify-end gap-3 border-t border-[#E5EAE7] bg-[#F8FAF9] px-6 py-4">
          <button
            type="button"
            onClick={onClose}
            disabled={loading}
            className="rounded-[10px] border border-[#E5EAE7] bg-white px-5 py-2.5 text-sm font-bold text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#212529] disabled:opacity-50"
          >
            Cancel
          </button>

          <button
            type="button"
            disabled={loading || !scheduledAt || !interviewerId || !location}
            onClick={handleSubmit}
            className="flex min-w-[140px] items-center justify-center rounded-[10px] bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:opacity-70"
          >
            {loading ? (
              <Loader2 size={16} className="animate-spin text-[#C3F53C]" />
            ) : (
              "Schedule Interview"
            )}
          </button>
        </div>
      </div>
    </div>
  );
}