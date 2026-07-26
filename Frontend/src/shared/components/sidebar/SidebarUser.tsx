import { ChevronDown } from "lucide-react";
import { useAuth } from "../../../app/hooks/useAuth";

export default function SidebarUser() {
  const { user } = useAuth();

  if (!user) return null;

  return (
    <div className="px-6 py-6 border-b border-[#E5EAE7]">
      <div className="flex flex-col items-center text-center">
        {/* User Initial Avatar */}
        <div className="relative flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-xl font-bold text-[#315343] shadow-sm mb-3">
          {user.fullName ? user.fullName.charAt(0).toUpperCase() : "H"}
          <div className="absolute bottom-0 right-1 h-3.5 w-3.5 rounded-full border-2 border-white bg-[#C3F53C]"></div>
        </div>

        {/* User Details */}
        <div className="flex items-center gap-1 cursor-pointer transition-opacity">
          <p className="truncate text-base font-semibold text-[#212529]">
            {user.fullName || "Harshit"}
          </p>
        </div>
        <p className="truncate text-xs font-medium text-[#75837D] capitalize mt-0.5">
          {user.role || "Developer"}
        </p>
      </div>
    </div>
  );
}