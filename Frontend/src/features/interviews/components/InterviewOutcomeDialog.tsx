import { useEffect, useState } from "react";
import { X, Loader2 } from "lucide-react";
import type { InterviewDetailed } from "../types/interview";
import { useRecordInterviewOutcome } from "../hooks/useRecordInterviewOutcome";
import type { InterviewStatus } from "../../../shared/types/recruitment";

interface InterviewOutcomeDialogProps {
  open: boolean;
  interview: InterviewDetailed | null;
  onClose: () => void;
  onSuccess: () => void;
}

export default function InterviewOutcomeDialog({
  open,
  interview,
  onClose,
  onSuccess,
}: InterviewOutcomeDialogProps) {
  const [status, setStatus] = useState<InterviewStatus>("Passed");
  const [feedback, setFeedback] = useState("");
  const [error, setError] = useState("");

  const { recordOutcome, loading } = useRecordInterviewOutcome();

  useEffect(() => {
    if (open) {
      setStatus("Passed");
      setFeedback("");
      setError("");
    }
  }, [open, interview]);

  if (!open || !interview) return null;

  async function handleSubmit(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!feedback.trim()) {
      setError("Feedback is required.");
      return;
    }

    setError("");
    const result = await recordOutcome(interview!.id, {
      status,
      feedback: feedback.trim(),
    });

    if (!result) {
      setError("Failed to record interview outcome.");
      return;
    }

    onSuccess();
    onClose();
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all">
      <div className="flex max-h-[90vh] w-full max-w-lg flex-col overflow-hidden rounded-2xl bg-white shadow-xl">
        
        {/* Header */}
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] px-6 py-4">
          <div>
            <h2 className="text-lg font-bold text-[#212529]">
              Record Outcome
            </h2>
            <p className="text-xs font-medium text-[#75837D]">
              {interview.candidateName}
            </p>
          </div>

          <button
            type="button"
            onClick={onClose}
            disabled={loading}
            className="rounded-lg p-2 text-[#75837D] transition-colors hover:bg-[#EEF3F0] hover:text-[#315343] disabled:opacity-50"
          >
            <X size={20} />
          </button>
        </div>

        {/* Form */}
        <form onSubmit={handleSubmit} className="flex min-h-0 flex-col">
          <div className="space-y-6 overflow-y-auto px-6 py-5">
            
            {/* Position Display */}
            <div className="rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-4">
              <p className="text-xs font-semibold uppercase tracking-wider text-[#75837D]">
                Position
              </p>
              <p className="mt-1 text-sm font-bold text-[#315343]">
                {interview.jobTitle}
              </p>
            </div>

            {/* Outcome Selection */}
            <div>
              <label className="mb-2.5 block text-sm font-semibold text-[#212529]">
                Final Outcome
              </label>

              <div className="grid grid-cols-2 gap-3">
                <button
                  type="button"
                  onClick={() => setStatus("Passed")}
                  disabled={loading}
                  className={`rounded-xl border px-4 py-3.5 text-sm font-bold transition-all ${
                    status === "Passed"
                      ? "border-[#315343] bg-[#EEF3F0] text-[#315343]"
                      : "border-[#E5EAE7] bg-white text-[#75837D] hover:border-[#315343]"
                  } disabled:cursor-not-allowed disabled:opacity-50`}
                >
                  Passed
                </button>

                <button
                  type="button"
                  onClick={() => setStatus("Failed")}
                  disabled={loading}
                  className={`rounded-xl border px-4 py-3.5 text-sm font-bold transition-all ${
                    status === "Failed"
                      ? "border-red-500 bg-red-50 text-red-700"
                      : "border-[#E5EAE7] bg-white text-[#75837D] hover:border-red-300"
                  } disabled:cursor-not-allowed disabled:opacity-50`}
                >
                  Failed
                </button>
              </div>
            </div>

            {/* Feedback */}
            <div>
              <label htmlFor="interview-feedback" className="mb-1.5 block text-sm font-semibold text-[#212529]">
                Feedback Notes
              </label>
              <textarea
                id="interview-feedback"
                value={feedback}
                onChange={(e) => setFeedback(e.target.value)}
                rows={4}
                maxLength={2000}
                disabled={loading}
                placeholder="Enter detailed feedback from the interview..."
                className="w-full resize-none rounded-[10px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343] disabled:cursor-not-allowed disabled:opacity-60"
              />
              <div className="mt-1 flex justify-end">
                <span className="text-xs font-medium text-[#75837D]">
                  {feedback.length}/2000
                </span>
              </div>
            </div>

            {error && (
              <p className="rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm font-medium text-red-700">
                {error}
              </p>
            )}
          </div>

          {/* Footer */}
          <div className="flex shrink-0 justify-end gap-3 border-t border-[#E5EAE7] bg-[#F8FAF9] px-6 py-4">
            <button
              type="button"
              onClick={onClose}
              disabled={loading}
              className="rounded-[10px] border border-[#E5EAE7] bg-white px-5 py-2.5 text-sm font-bold text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#212529] disabled:pointer-events-none disabled:opacity-50"
            >
              Cancel
            </button>

            <button
              type="submit"
              disabled={loading || !feedback.trim()}
              className="flex min-w-[130px] items-center justify-center rounded-[10px] bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:pointer-events-none disabled:opacity-50"
            >
              {loading ? (
                <Loader2 size={16} className="animate-spin text-[#C3F53C]" />
              ) : (
                "Save Outcome"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}