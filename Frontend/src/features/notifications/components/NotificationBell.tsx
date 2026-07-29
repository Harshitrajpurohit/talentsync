import { Bell } from "lucide-react";
import { NavLink } from "react-router-dom";
import { useEffect, useState } from "react";

import { useUnreadCount } from "../hooks/useUnreadCount";
import { useNotificationHub } from "../hooks/useNotificationHub";

export default function NotificationBell() {
  const { unreadCount } = useUnreadCount();
  const [count, setCount] = useState<number>(0);

  useEffect(() => {
    setCount(unreadCount);
  }, [unreadCount]);

  useNotificationHub({
    onReceive: () => {
      setCount((prev) => prev + 1);
    },
  });

  return (
    <NavLink
      to="/notifications"
      className={({ isActive }) =>
        `relative flex h-10 w-10 items-center justify-center rounded-full shadow-sm transition-all duration-300 ${
          isActive
            ? "bg-[#315343] text-[#C3F53C]"
            : "bg-white text-[#212529] hover:bg-[#315343] hover:text-[#C3F53C]"
        }`
      }
    >
      <Bell size={20} />

      {count > 0 && (
        <span className="absolute -right-1 -top-1 flex h-5 min-w-5 items-center justify-center rounded-full bg-[#C3F53C] px-1 text-[10px] font-bold text-[#315343] shadow-sm">
          {count > 99 ? "99+" : count}
        </span>
      )}
    </NavLink>
  );
}