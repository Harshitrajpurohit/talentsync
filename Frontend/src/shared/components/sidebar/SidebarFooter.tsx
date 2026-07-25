import { LogOut } from "lucide-react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../app/hooks/useAuth";

export default function SidebarFooter() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  function handleLogout() {
    logout();
    navigate("/login", { replace: true });
  }

  return (
    <div className="border-t border-slate-800/80 p-3">
      <button
        onClick={handleLogout}
        className="flex w-full items-center gap-3 rounded-xl px-3.5 py-2.5 text-sm font-medium text-red-400 transition hover:bg-red-500/10 hover:text-red-300"
      >
        <LogOut size={18} className="shrink-0" />
        <span>Logout</span>
      </button>
    </div>
  );
}