import { useState } from "react";
import { X, Loader2 } from "lucide-react";
import { useCancelInterview } from "../../../../interviews/hooks/useCancelInterview";

interface CancelInterviewDialogProps {
  open: boolean;
  interviewId: string;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function CancelInterviewDialog({
  open,
  interviewId,
  onClose,
  onSuccess,
}: CancelInterviewDialogProps) {
  const [reason, setReason] = useState("");
const [error, setError] = useState("");
  const { cancelInterview, loading } = useCancelInterview();

  if (!open) return null;

    async function handleSubmit() {
        if (!reason.trim()) {
            setError("Cancellation reason is required.");
            return;
        }

        setError("");

        const response = await cancelInterview(interviewId, {
            status: "Cancelled",
            feedback: reason.trim(),
        });

        if (!response) return;

        onSuccess?.();
        setReason("");
        setError("");
        onClose();
    }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all">
      <div className="flex max-h-[90vh] w-full max-w-lg flex-col overflow-hidden rounded-2xl bg-white shadow-xl">
        
        {/* Header */}
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] px-6 py-4">
          <div>
            <h2 className="text-lg font-bold text-[#212529]">
              Cancel Interview
            </h2>
            <p className="text-xs font-medium text-[#75837D]">
              Are you sure you want to cancel this interview?
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
        <div className="overflow-y-auto p-6">
          <div className="rounded-xl border border-red-200 bg-red-50 p-4 mb-6">
            <p className="text-sm font-bold text-red-700">Warning</p>
            <p className="mt-1 text-xs font-medium text-red-600">
              This action cannot be undone. The candidate will be notified of the cancellation.
            </p>
          </div>

          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
              Reason (Optional)
            </label>
            <textarea
                rows={4}
                required
                value={reason}
                onChange={(e) => {
                    setReason(e.target.value);

                    if (error && e.target.value.trim()) {
                    setError("");
                    }
                }}
                className={`w-full resize-none rounded-[10px] border bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition ${
                    error
                    ? "border-red-500 ring-1 ring-red-500"
                    : "border-[#E5EAE7] focus:border-red-500 focus:ring-1 focus:ring-red-500"
                }`}
                placeholder="Provide a reason for the cancellation..."
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
            Close
          </button>

          <button
            type="button"
            disabled={loading}
            onClick={handleSubmit}
            className="flex min-w-[150px] items-center justify-center rounded-[10px] bg-red-600 px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-red-600/20 transition-all hover:bg-red-700 active:scale-95 disabled:opacity-50"
          >
            {loading ? (
              <Loader2 size={16} className="animate-spin text-white" />
            ) : (
              "Cancel Interview"
            )}
          </button>
        </div>
      </div>
    </div>
  );
}