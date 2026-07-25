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
        `flex items-center gap-3 rounded-xl px-3.5 py-2.5 text-sm font-medium transition-all ${
          isActive
            ? "bg-emerald-500 text-slate-950 font-semibold shadow-md shadow-emerald-500/20"
            : "text-slate-400 hover:bg-slate-900/80 hover:text-white"
        }`
      }
    >
      <Icon size={18} className="shrink-0" />
      <span className="truncate">{item.title}</span>
    </NavLink>
  );
}