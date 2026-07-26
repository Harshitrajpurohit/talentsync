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

  const items = navigation[user.role];

  return (
    <>
      {/* Mobile Header (Stacks on top due to DashboardLayout flex-col) */}
      <header className="sticky top-0 z-30 flex h-16 shrink-0 items-center justify-between border-b border-[#E5EAE7] bg-white px-4 lg:hidden">
        <SidebarHeader compact />

        <button
          onClick={() => setIsOpen(true)}
          className="rounded-lg p-2 text-[#75837D] hover:bg-[#EEF3F0] hover:text-[#212529]"
        >
          <Menu size={22} />
        </button>
      </header>

      {/* Overlay */}
      {isOpen && (
        <div
          className="fixed inset-0 z-40 bg-black/40 backdrop-blur-sm lg:hidden"
          onClick={() => setIsOpen(false)}
        />
      )}

      {/* Sidebar */}
      <aside
        className={`
          fixed inset-y-0 left-0 z-50
          flex w-[260px] flex-col
          bg-white border-r border-[#E5EAE7]
          transition-transform duration-300 ease-in-out

          ${isOpen ? "translate-x-0" : "-translate-x-full"}

          lg:static
          lg:translate-x-0
          lg:flex
        `}
      >
        {/* Mobile Close Button (The Cross) */}
        <div className="flex items-center justify-between border-b border-[#E5EAE7] px-4 py-4 lg:hidden">
          <SidebarHeader compact />

          <button
            onClick={() => setIsOpen(false)}
            className="rounded-lg p-2 text-[#75837D] hover:bg-[#EEF3F0] hover:text-[#212529]"
          >
            <X size={22} />
          </button>
        </div>

        {/* Desktop Header */}
        <div className="hidden lg:block pt-6 pb-2 px-6">
          <SidebarHeader />
        </div>

        <SidebarUser />

        <nav className="flex-1 overflow-y-auto px-4 py-4">
          <div className="space-y-1.5">
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