import { NavLink } from "react-router-dom";
import type { SidebarItem as SidebarItemType } from "./types";

interface Props {
  item: SidebarItemType;
  onClick?: () => void;
}

export default function SidebarItem({ item, onClick }: Props) {
  const Icon = item.icon;

  return (
    <NavLink
      to={item.path}
      end
      onClick={onClick}
      className={({ isActive }) =>
        `flex items-center gap-3 rounded-[10px] px-4 py-3 text-sm font-medium transition-all ${
          isActive
            ? "bg-[#315343] text-white shadow-sm"
            : "text-[#75837D] hover:bg-[#EEF3F0] hover:text-[#212529]"
        }`
      }
    >
      <Icon size={20} className="shrink-0" />
      <span className="truncate">{item.title}</span>
    </NavLink>
  );
}