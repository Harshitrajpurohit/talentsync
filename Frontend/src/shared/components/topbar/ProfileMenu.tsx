import { ChevronDown } from "lucide-react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../../../app/hooks/useAuth";

export default function ProfileMenu() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const [open, setOpen] = useState(false);

  if (!user) return null;

  function handleLogout() {
    logout();
    navigate("/login");
  }

  return (
    <div className="relative">
      <button
        onClick={() => setOpen((value) => !value)}
        className="flex items-center gap-2.5 rounded-full bg-white pl-1.5 pr-4 py-1.5 shadow-sm transition hover:bg-[#E5EAE7]"
      >
        <div className="flex h-8 w-8 items-center justify-center rounded-full bg-[#EEF3F0] text-sm font-bold text-[#315343]">
          {user.fullName ? user.fullName.charAt(0).toUpperCase() : "H"}
        </div>

        <div className="hidden text-left lg:block">
          <p className="text-sm font-semibold text-[#212529]">
            {user.fullName || "Harshit"}
          </p>
        </div>

        <ChevronDown
          size={16}
          className={`hidden text-[#75837D] transition lg:block ${
            open ? "rotate-180" : ""
          }`}
        />
      </button>

      {open && (
        <div className="absolute right-0 mt-2 w-52 overflow-hidden rounded-xl border border-[#E5EAE7] bg-white shadow-lg z-50">
          <NavItem
            title="My Profile"
            onClick={() => {
              navigate(`/${user.role?.toLowerCase() || "developer"}/profile`);
              setOpen(false);
            }}
          />

          <NavItem
            title="Settings"
            onClick={() => {
              navigate(`/${user.role?.toLowerCase() || "developer"}/settings`);
              setOpen(false);
            }}
          />

          <hr className="border-[#E5EAE7]" />

          <button
            onClick={handleLogout}
            className="w-full px-4 py-3 text-left text-sm font-medium text-red-500 transition hover:bg-[#EEF3F0]"
          >
            Logout
          </button>
        </div>
      )}
    </div>
  );
}

interface NavItemProps {
  title: string;
  onClick: () => void;
}

function NavItem({ title, onClick }: NavItemProps) {
  return (
    <button
      onClick={onClick}
      className="w-full px-4 py-3 text-left text-sm font-medium text-[#75837D] transition hover:bg-[#EEF3F0] hover:text-[#212529]"
    >
      {title}
    </button>
  );
}