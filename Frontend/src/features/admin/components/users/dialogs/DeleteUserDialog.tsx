import { AlertTriangle, RotateCcw, Trash2, X, Loader2 } from "lucide-react";
import type { UserWithRole } from "../../../types/user";
import { useUserDeletion } from "../../../hooks/users/useUserDeletion";

interface DeleteUserDialogProps {
  user: UserWithRole;
  onClose: () => void;
  onSuccess: () => void;
}

export default function DeleteUserDialog({
  user,
  onClose,
  onSuccess,
}: DeleteUserDialogProps) {
  const { deleteUser, restoreUser, loading, error } = useUserDeletion();
  const isDeleted = user.isDeleted;

  const handleSubmit = async () => {
    try {
      if (isDeleted) {
        await restoreUser(user.userId);
      } else {
        await deleteUser(user.userId);
      }
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
            <div
              className={`flex h-10 w-10 shrink-0 items-center justify-center rounded-full ${
                isDeleted ? "bg-[#EEF3F0] text-[#315343]" : "bg-red-50 text-red-600"
              }`}
            >
              {isDeleted ? <RotateCcw size={20} /> : <Trash2 size={20} />}
            </div>
            <div>
              <h2 className="text-lg font-bold text-[#212529]">
                {isDeleted ? "Restore User" : "Delete User"}
              </h2>
              <p className="mt-0.5 text-xs font-medium text-[#75837D]">
                {isDeleted
                  ? "Restore this user's account access."
                  : "Soft delete this user's account."}
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

        {/* Content */}
        <div className="space-y-6 overflow-y-auto px-6 py-5">
          {/* User Info */}
          <div className="rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-4">
            <p className="text-xs font-semibold uppercase tracking-wider text-[#75837D]">
              User
            </p>
            <p className="mt-1 text-sm font-bold text-[#212529]">{user.name}</p>
            <p className="mt-0.5 text-xs font-medium text-[#75837D]">{user.email}</p>
          </div>

          {/* Warning Message */}
          <div
            className={`rounded-xl border p-4 ${
              isDeleted ? "border-[#E5EAE7] bg-white" : "border-red-200 bg-red-50"
            }`}
          >
            <div className="flex items-start gap-3">
              {!isDeleted && <AlertTriangle size={18} className="mt-0.5 shrink-0 text-red-600" />}
              <p
                className={`text-sm font-medium leading-relaxed ${
                  isDeleted ? "text-[#75837D]" : "text-red-800"
                }`}
              >
                {isDeleted
                  ? "This user is currently soft deleted. Restoring the account will make it active in the system again."
                  : "This will soft delete the user. The account will remain in the database and can be restored later."}
              </p>
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
            type="button"
            onClick={handleSubmit}
            disabled={loading}
            className={`flex min-w-[130px] items-center justify-center rounded-[10px] px-5 py-2.5 text-sm font-bold transition-all active:scale-95 disabled:pointer-events-none disabled:opacity-50 ${
              isDeleted
                ? "bg-[#315343] text-white shadow-sm shadow-[#315343]/20 hover:bg-[#C3F53C] hover:text-[#315343]"
                : "bg-red-600 text-white shadow-sm shadow-red-600/20 hover:bg-red-700"
            }`}
          >
            {loading ? (
              <Loader2
                size={16}
                className={`animate-spin ${isDeleted ? "text-[#C3F53C]" : "text-white"}`}
              />
            ) : isDeleted ? (
              "Restore User"
            ) : (
              "Delete User"
            )}
          </button>
        </div>
      </div>
    </div>
  );
}