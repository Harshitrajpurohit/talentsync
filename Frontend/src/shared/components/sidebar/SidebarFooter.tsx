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
    <div className="px-6 pb-6 pt-4">
      {/* Progress Bar (from the design) */}
      {/* <div className="mb-6">
        <div className="flex justify-between items-center mb-2">
          <span className="text-sm font-bold text-[#212529]">87%</span>
        </div>
        <div className="h-1.5 w-full rounded-full bg-[#EEF3F0] overflow-hidden">
          <div className="h-full bg-[#315343] w-[87%] relative">
             <div className="absolute right-0 top-0 h-full w-1/4 bg-[#C3F53C]"></div>
          </div>
        </div>
        <p className="mt-2 text-xs font-medium text-[#75837D]">
          Profile Complete
        </p>
      </div> */}

      <button
        onClick={handleLogout}
        className="flex w-full items-center gap-2 rounded-[10px] px-2 py-2 text-sm font-semibold text-red-500 transition-colors hover:bg-[#EEF3F0]"
      >
        <LogOut size={18} className="shrink-0" />
        <span>Logout</span>
      </button>
    </div>
  );
}