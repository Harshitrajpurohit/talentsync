import { Activity, X, Loader2 } from "lucide-react";
import type { UserWithRole } from "../../../types/user";
import type { UserStatus } from "../../../../../shared/types/user";
import { useChangeUserStatus } from "../../../hooks/users/useChangeUserStatus";

interface ChangeStatusDialogProps {
  user: UserWithRole;
  onClose: () => void;
  onSuccess: () => void;
}

const statuses: UserStatus[] = ["Active", "Inactive", "Suspended"];

export default function ChangeStatusDialog({
  user,
  onClose,
  onSuccess,
}: ChangeStatusDialogProps) {
  const { changeStatus, loading, error } = useChangeUserStatus();

  const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    const formData = new FormData(event.currentTarget);
    const status = formData.get("status") as UserStatus;

    if (!status || status === user.status) {
      onClose();
      return;
    }

    try {
      await changeStatus(user.userId, status);
      onSuccess();
      onClose();
    } catch {
      // Handled by hook
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-[#212529]/40 p-4 backdrop-blur-sm transition-all">
      <div className="flex max-h-[90vh] w-full max-w-lg flex-col overflow-hidden rounded-2xl bg-white shadow-xl">
        {/* Header */}
        <div className="flex shrink-0 items-start justify-between border-b border-[#E5EAE7] px-6 py-5">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
              <Activity size={20} />
            </div>
            <div>
              <h2 className="text-lg font-bold text-[#212529]">
                Change Status
              </h2>
              <p className="mt-0.5 text-xs font-medium text-[#75837D]">
                Update this user's account status.
              </p>
            </div>
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

        {/* Form Content */}
        <form onSubmit={handleSubmit} className="flex min-h-0 flex-col">
          <div className="space-y-6 overflow-y-auto px-6 py-5">
            {/* User Info */}
            <div className="rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-4">
              <p className="text-xs font-semibold uppercase tracking-wider text-[#75837D]">
                User
              </p>
              <p className="mt-1 text-sm font-bold text-[#212529]">{user.name}</p>
              <p className="mt-0.5 text-xs font-medium text-[#75837D]">{user.email}</p>
            </div>

            <div className="grid gap-5 sm:grid-cols-2">
              {/* Current Status */}
              <div>
                <label className="mb-1.5 block text-sm font-semibold text-[#212529]">
                  Current Status
                </label>
                <div className="flex h-[42px] items-center rounded-[10px] border border-[#E5EAE7] bg-[#F8FAF9] px-4 text-sm font-medium text-[#75837D]">
                  {user.status}
                </div>
              </div>

              {/* New Status */}
              <div>
                <label htmlFor="status" className="mb-1.5 block text-sm font-semibold text-[#212529]">
                  New Status
                </label>
                <div className="relative">
                  <select
                    id="status"
                    name="status"
                    defaultValue={user.status}
                    disabled={loading}
                    className="w-full appearance-none rounded-[10px] border border-[#E5EAE7] bg-white py-2.5 pl-4 pr-10 text-sm font-medium text-[#212529] outline-none transition-all focus:border-[#315343] focus:ring-1 focus:ring-[#315343] disabled:cursor-not-allowed disabled:bg-[#F8FAF9] disabled:opacity-60"
                  >
                    {statuses.map((status) => (
                      <option key={status} value={status}>
                        {status}
                      </option>
                    ))}
                  </select>
                  <div className="pointer-events-none absolute right-4 top-1/2 -translate-y-1/2 text-[#75837D]">
                    <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" viewBox="0 0 24 24" fill="none" stroke="currentColor" strokeWidth="2" strokeLinecap="round" strokeLinejoin="round"><path d="m6 9 6 6 6-6"/></svg>
                  </div>
                </div>
              </div>
            </div>

            {error && (
              <div className="rounded-xl border border-red-200 bg-red-50 px-4 py-3">
                <p className="text-sm font-semibold text-red-700">{error}</p>
              </div>
            )}
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
              type="submit"
              disabled={loading}
              className="flex min-w-[140px] items-center justify-center rounded-[10px] bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm shadow-[#315343]/20 transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 disabled:pointer-events-none disabled:opacity-50"
            >
              {loading ? (
                <Loader2 size={16} className="animate-spin text-[#C3F53C]" />
              ) : (
                "Update Status"
              )}
            </button>
          </div>
        </form>
      </div>
    </div>
  );
}