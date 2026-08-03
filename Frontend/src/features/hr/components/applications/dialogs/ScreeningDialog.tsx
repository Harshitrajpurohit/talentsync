import { useState } from "react";
import { X, Loader2 } from "lucide-react";
import { useCreateScreening } from "../../../../../features/screenings/hooks/useCreateScreening";
import type {
  CreateScreeningRequest,
  ScreeningResult,
} from "../../../../../features/screenings/types/screening";

interface ScreeningDialogProps {
  open: boolean;
  applicationId: string;
  onClose: () => void;
  onSuccess?: () => void;
}

export default function ScreeningDialog({
  open,
  applicationId,
  onClose,
  onSuccess,
}: ScreeningDialogProps) {
  const { createScreening, loading, error } = useCreateScreening();

  const [result, setResult] = useState<ScreeningResult>("Pass");
  const [notes, setNotes] = useState("");

  if (!open) return null;

  async function handleSubmit() {
    if (!notes.trim()) return;

    const request: CreateScreeningRequest = {
      applicationId,
      result,
      notes,
    };

    const created = await createScreening(request);

    if (created) {
      onSuccess?.();
      setNotes("");
      setResult("Pass");
      onClose();
    }
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all">
      <div className="flex max-h-[90vh] w-full max-w-lg flex-col overflow-hidden rounded-2xl bg-white shadow-xl">
        <div className="flex shrink-0 items-center justify-between border-b border-[#E5EAE7] px-6 py-4">
          <div>
            <h2 className="text-lg font-bold text-[#212529]">
              Screening Result
            </h2>
            <p className="text-xs font-medium text-[#75837D]">
              Record the outcome of the candidate's screening.
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

        <div className="space-y-6 overflow-y-auto p-6">
          {error && (
            <div className="rounded-xl border border-red-200 bg-red-50 p-3 text-sm font-medium text-red-600">
              {error}
            </div>
          )}

          <div>
            <label className="mb-2.5 block text-sm font-semibold text-[#212529]">
              Outcome
            </label>

            <div className="grid gap-3 sm:grid-cols-2">
              <label
                className={`flex cursor-pointer items-start gap-3 rounded-xl border p-4 transition-colors ${
                  result === "Pass"
                    ? "border-[#315343] bg-[#EEF3F0]"
                    : "border-[#E5EAE7] hover:border-[#315343]"
                }`}
              >
                <input
                  type="radio"
                  className="mt-0.5 h-4 w-4 shrink-0 text-[#315343] focus:ring-[#315343]"
                  checked={result === "Pass"}
                  onChange={() => setResult("Pass")}
                />
                <div>
                  <p className="text-sm font-bold text-[#212529]">Pass</p>
                  <p className="mt-0.5 text-xs font-medium text-[#75837D]">
                    Eligible for interview.
                  </p>
                </div>
              </label>

              <label
                className={`flex cursor-pointer items-start gap-3 rounded-xl border p-4 transition-colors ${
                  result === "Fail"
                    ? "border-red-500 bg-red-50"
                    : "border-[#E5EAE7] hover:border-red-300"
                }`}
              >
                <input
                  type="radio"
                  className="mt-0.5 h-4 w-4 shrink-0 text-red-600 focus:ring-red-600"
                  checked={result === "Fail"}
                  onChange={() => setResult("Fail")}
                />
                <div>
                  <p className="text-sm font-bold text-[#212529]">Fail</p>
                  <p className="mt-0.5 text-xs font-medium text-[#75837D]">
                    Reject candidate.
                  </p>
                </div>
              </label>
            </div>
          </div>

          <div>
            <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
              Notes & Feedback
            </label>
            <textarea
              rows={4}
              value={notes}
              onChange={(e) => setNotes(e.target.value)}
              placeholder="Add your screening remarks here..."
              className="w-full resize-none rounded-[10px] border border-[#E5EAE7] bg-white px-4 py-2.5 text-sm text-[#212529] outline-none transition focus:border-[#315343] focus:ring-1 focus:ring-[#315343]"
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
            disabled={loading || !notes.trim()}
            onClick={handleSubmit}
            className="flex min-w-[120px] items-center justify-center rounded-[10px] bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:opacity-70"
          >
            {loading ? (
              <Loader2 size={16} className="animate-spin text-[#C3F53C]" />
            ) : (
              "Submit"
            )}
          </button>
        </div>
      </div>
    </div>
  );
}