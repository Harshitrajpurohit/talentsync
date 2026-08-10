import { useState } from "react";
import { X, Loader2 } from "lucide-react";
import { useManagers } from "../../../hooks/userrole/useManagers";
import { useRescheduleInterview } from "../../../../interviews/hooks/useRescheduleInterview";

interface RescheduleInterviewDialogProps {
  open: boolean;
  interviewId: string;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function RescheduleInterviewDialog({
  open,
  interviewId,
  onClose,
  onSuccess,
}: RescheduleInterviewDialogProps) {
  const { managers } = useManagers();
  const { rescheduleInterview, loading } = useRescheduleInterview();

  const [date, setDate] = useState("");
  const [time, setTime] = useState("");
  const [location, setLocation] = useState("");
  const [interviewerId, setInterviewerId] = useState("");

  if (!open) return null;

  async function handleSubmit() {
    if (!date || !time || !location || !interviewerId) return;

    const scheduledAt = `${date}T${time}`;

    const response = await rescheduleInterview(interviewId, {
      scheduledAt,
      interviewerId,
      location,
    });

    if (!response) return;

    onSuccess?.();
    onClose();
  }

  const inputClass =
    "w-full rounded-[10px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]";
  const labelClass = "mb-1.5 block text-sm font-semibold text-[#212529]";

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all">
      <div className="flex max-h-[90vh] w-full max-w-lg flex-col overflow-hidden rounded-2xl bg-white shadow-xl">
        
        {/* Header */}
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] px-6 py-4">
          <div>
            <h2 className="text-lg font-bold text-[#212529]">
              Reschedule Interview
            </h2>
            <p className="text-xs font-medium text-[#75837D]">
              Update the date, time, location, or interviewer.
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

        {/* Content */}
        <div className="space-y-5 overflow-y-auto p-6">
          <div className="grid grid-cols-2 gap-5">
            <div>
              <label className={labelClass}>Date</label>
              <input
                type="date"
                value={date}
                onChange={(e) => setDate(e.target.value)}
                className={inputClass}
              />
            </div>

            <div>
              <label className={labelClass}>Time</label>
              <input
                type="time"
                value={time}
                onChange={(e) => setTime(e.target.value)}
                className={inputClass}
              />
            </div>
          </div>

          <div>
            <label className={labelClass}>Interviewer</label>
            <div className="relative">
              <select
                value={interviewerId}
                onChange={(e) => setInterviewerId(e.target.value)}
                className={`${inputClass} appearance-none cursor-pointer pr-10`}
              >
                <option value="" disabled>Select Interviewer</option>
                {managers?.map((manager) => (
                  <option key={manager.userId} value={manager.userId}>
                    {manager.userName}
                  </option>
                ))}
              </select>
              <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6"/></svg>
              </div>
            </div>
          </div>

          <div>
            <label className={labelClass}>Location / Link</label>
            <input
              value={location}
              onChange={(e) => setLocation(e.target.value)}
              className={inputClass}
              placeholder="e.g. Meeting Room / Google Meet / Teams"
            />
          </div>
        </div>

        {/* Footer */}
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
            disabled={loading || !date || !time || !location || !interviewerId}
            onClick={handleSubmit}
            className="flex min-w-[140px] items-center justify-center rounded-[10px] bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:opacity-70"
          >
            {loading ? (
              <Loader2 size={16} className="animate-spin text-[#C3F53C]" />
            ) : (
              "Save Changes"
            )}
          </button>
        </div>
      </div>
    </div>
  );
}