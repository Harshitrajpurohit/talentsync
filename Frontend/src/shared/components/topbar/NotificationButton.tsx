import { Bell } from "lucide-react";
import { NavLink } from "react-router-dom";
import { useAuth } from "../../../app/hooks/useAuth";

export default function NotificationButton() {
  const { user } = useAuth();

  if (!user) return null;

  return (
    <NavLink
      to={`/notifications`}
      className={({ isActive }) =>
        `relative flex h-10 w-10 items-center justify-center rounded-full transition-colors shadow-sm ${
          isActive
            ? "bg-[#315343] text-white"
            : "bg-white text-[#212529] hover:bg-[#E5EAE7]"
        }`
      }
    >
      <Bell size={20} />

      {/* unread notification dot */}
      <span className="absolute right-2.5 top-2.5 h-2 w-2 rounded-full bg-red-500 border border-white" />
    </NavLink>
  );
}