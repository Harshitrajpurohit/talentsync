import {
  Eye,
  MoreHorizontal,
  ShieldCheck,
  Activity,
  Trash2,
} from "lucide-react";

import { useNavigate } from "react-router-dom";
import { useState } from "react";

import type { UserWithRole } from "../../types/user";

interface UserActionsProps {
  user: UserWithRole;

  onChangeRole: (user: UserWithRole) => void;
  onChangeStatus: (user: UserWithRole) => void;
  onDelete: (user: UserWithRole) => void;
}

export default function UserActions({
  user,
  onChangeRole,
  onChangeStatus,
  onDelete,
}: UserActionsProps) {
  const navigate = useNavigate();

  const [open, setOpen] = useState(false);

  const closeMenu = () => {
    setOpen(false);
  };

  return (
    <div className="relative flex justify-end">
      {/* Trigger */}
      <button
        type="button"
        onClick={() => setOpen((value) => !value)}
        className="flex h-9 w-9 items-center justify-center rounded-full text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#315343]"
        aria-label="User actions"
        aria-expanded={open}
      >
        <MoreHorizontal size={19} />
      </button>

      {open && (
        <>
          {/* Outside click */}
          <button
            type="button"
            aria-label="Close actions"
            className="fixed inset-0 z-10 cursor-default"
            onClick={closeMenu}
          />

          {/* Menu */}
          <div className="absolute right-0 top-11 z-20 w-48 overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white p-1.5 shadow-lg">
            {/* View Details */}
            <button
              type="button"
              onClick={() => {
                closeMenu();

                navigate(`/admin/users/${user.userId}`);
              }}
              className="flex w-full items-center gap-2.5 rounded-xl px-3 py-2.5 text-left text-sm font-semibold text-[#212529] transition hover:bg-[#F8FAF9] hover:text-[#315343]"
            >
              <Eye size={16} />

              View Details
            </button>

            {/* Change Role */}
            <button
              type="button"
              onClick={() => {
                closeMenu();
                onChangeRole(user);
              }}
              className="flex w-full items-center gap-2.5 rounded-xl px-3 py-2.5 text-left text-sm font-semibold text-[#212529] transition hover:bg-[#F8FAF9] hover:text-[#315343]"
            >
              <ShieldCheck size={16} />

              Change Role
            </button>

            {/* Change Status */}
            <button
              type="button"
              onClick={() => {
                closeMenu();
                onChangeStatus(user);
              }}
              className="flex w-full items-center gap-2.5 rounded-xl px-3 py-2.5 text-left text-sm font-semibold text-[#212529] transition hover:bg-[#F8FAF9] hover:text-[#315343]"
            >
              <Activity size={16} />

              Change Status
            </button>

            {/* Delete / Restore */}
            <button
              type="button"
              onClick={() => {
                closeMenu();
                onDelete(user);
              }}
              className={`flex w-full items-center gap-2.5 rounded-xl px-3 py-2.5 text-left text-sm font-semibold transition ${
                user.isDeleted
                  ? "text-[#315343] hover:bg-[#F8FAF9]"
                  : "text-red-600 hover:bg-red-50"
              }`}
            >
              <Trash2 size={16} />

              {user.isDeleted
                ? "Restore User"
                : "Delete User"}
            </button>
          </div>
        </>
      )}
    </div>
  );
}