import { Link } from "react-router-dom";
import { User, Upload, Search, FileText } from "lucide-react";
import type { LucideIcon } from "lucide-react";

interface ActionItem {
  title: string;
  icon: LucideIcon;
  to: string;
}

export default function QuickActions() {
  const actions: ActionItem[] = [
    { title: "Update Profile", icon: User, to: "/candidate/profile" },
    { title: "Upload Resume", icon: Upload, to: "/candidate/profile" },
    { title: "Search Jobs", icon: Search, to: "/candidate/jobs" },
    { title: "My Applications", icon: FileText, to: "/candidate/applications" },
  ];

  return (
    <div className="flex flex-col justify-between rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <h2 className="mb-5 text-lg font-bold text-[#212529] dark:text-white">
        Quick Actions
      </h2>

      <div className="grid grid-cols-2 gap-3">
        {actions.map((action) => {
          const Icon = action.icon;

          return (
            <Link
              key={action.title}
              to={action.to}
              className="group flex flex-col items-center justify-center rounded-xl bg-[#F8FAF9] p-4 text-center transition-all duration-300 hover:border-[#E5EAE7] hover:bg-[#E5EAE7]/40 hover:shadow-sm dark:bg-[#1e3329] dark:hover:bg-[#1e3329]/80"
            >
              <div className="mb-2.5 flex h-10 w-10 items-center justify-center rounded-full bg-white text-[#75837D] shadow-sm transition-all duration-300 group-hover:-translate-y-0.5 group-hover:bg-[#C3F53C] group-hover:text-[#315343] dark:bg-[#253f33] dark:text-gray-300 dark:group-hover:bg-[#C3F53C] dark:group-hover:text-[#315343]">
                <Icon size={18} strokeWidth={1.75} />
              </div>

              <span className="text-xs font-bold text-[#212529] transition-colors group-hover:text-[#315343] dark:text-white/90 dark:group-hover:text-[#C3F53C]">
                {action.title}
              </span>
            </Link>
          );
        })}
      </div>
    </div>
  );
}