import { useState } from "react";
import { Menu, X } from "lucide-react";
import { useAuth } from "../../../app/hooks/useAuth";
import { navigation } from "./navigation";
import SidebarHeader from "./SidebarHeader";
import SidebarUser from "./SidebarUser";
import SidebarItem from "./SidebarItem";
import SidebarFooter from "./SidebarFooter";

export default function Sidebar() {
  const { user } = useAuth();
  const [isOpen, setIsOpen] = useState(false);

  if (!user) return null;

  const items = navigation[user.role] || [];

  return (
    <>
      {/* Mobile Top Navigation Bar */}
      <div className="flex items-center justify-between border-b border-slate-800 bg-slate-950 px-4 py-3 lg:hidden">
        <SidebarHeader compact />
        <button
          onClick={() => setIsOpen(!isOpen)}
          className="rounded-lg border border-slate-800 bg-slate-900/80 p-2 text-slate-400 hover:bg-slate-800 hover:text-white"
          aria-label="Toggle navigation"
        >
          {isOpen ? <X size={20} /> : <Menu size={20} />}
        </button>
      </div>

      {/* Mobile Backdrop Overlay */}
      {isOpen && (
        <div
          onClick={() => setIsOpen(false)}
          className="fixed inset-0 z-40 bg-slate-950/80 backdrop-blur-sm lg:hidden"
        />
      )}

      {/* Sidebar Drawer */}
      <aside
        className={`fixed bottom-0 top-0 z-50 flex w-72 flex-col border-r border-slate-800/80 bg-slate-950 transition-transform duration-300 ease-in-out lg:static lg:translate-x-0 ${
          isOpen ? "left-0 translate-x-0" : "-translate-x-full lg:translate-x-0"
        }`}
      >
        <div className="hidden lg:block">
          <SidebarHeader />
        </div>

        <SidebarUser />

        {/* Navigation Items */}
        <nav className="flex-1 overflow-y-auto px-3 py-4">
          <div className="space-y-1">
            {items.map((item) => (
              <SidebarItem
                key={item.path}
                item={item}
                onClick={() => setIsOpen(false)}
              />
            ))}
          </div>
        </nav>

        <SidebarFooter />
      </aside>
    </>
  );
}