import type { UserStatus } from "../../../../shared/types/user";

interface UserStatusBadgeProps {
  status: UserStatus;
}

export default function UserStatusBadge({ status }: UserStatusBadgeProps) {
  const styles: Record<UserStatus, string> = {
    Active: "bg-[#C3F53C]/20 text-[#315343]",
    Inactive: "bg-[#F1F3F2] text-[#75837D]",
    Suspended: "bg-amber-100 text-amber-800",
    Deleted: "bg-red-100 text-red-700",
  };

  return (
    <span
      className={`inline-flex items-center gap-1.5 rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${styles[status]}`}
    >
      <span
        className={`h-1.5 w-1.5 rounded-full ${
          status === "Active"
            ? "bg-[#315343]"
            : status === "Suspended"
              ? "bg-amber-600"
              : status === "Deleted"
                ? "bg-red-600"
                : "bg-[#75837D]"
        }`}
      />
      {status}
    </span>
  );
}