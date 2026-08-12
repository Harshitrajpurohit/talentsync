import type { UserRole } from "../../../../shared/types/role";

interface UserRoleBadgeProps {
  role: UserRole;
}

export default function UserRoleBadge({ role }: UserRoleBadgeProps) {
  return (
    <span className="inline-flex rounded-md bg-[#EEF3F0] px-2.5 py-1 text-xs font-bold uppercase tracking-wider text-[#315343]">
      {role}
    </span>
  );
}