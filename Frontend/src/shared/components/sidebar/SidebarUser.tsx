import { useAuth } from "../../../app/hooks/useAuth";

export default function SidebarUser() {
  const { user } = useAuth();

  if (!user) return null;

  return (
    <div className="border-b border-slate-800/80 p-4">
      <div className="flex items-center gap-3 rounded-xl border border-slate-800 bg-slate-900/60 p-3 backdrop-blur-sm">
        {/* User Initial Avatar */}
        <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-emerald-500 font-semibold text-slate-950 shadow-md shadow-emerald-500/10">
          {user.fullName ? user.fullName.charAt(0).toUpperCase() : "U"}
        </div>

        {/* User Details */}
        <div className="min-w-0 flex-1">
          <p className="truncate text-sm font-semibold text-white">
            {user.fullName}
          </p>
          <p className="truncate text-xs font-medium text-emerald-400 capitalize">
            {user.role}
          </p>
        </div>
      </div>
    </div>
  );
}