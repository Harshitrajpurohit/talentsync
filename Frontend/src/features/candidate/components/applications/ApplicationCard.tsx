import { CalendarDays, Briefcase } from "lucide-react";

import type { ApplicationWithDetails } from "../../types/application";
import ApplicationStatusBadge from "./ApplicationStatusBadge";

interface ApplicationCardProps {
  application: ApplicationWithDetails;
}

export default function ApplicationCard({
  application,
}: ApplicationCardProps) {
  return (
    <div className="group flex flex-col rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm transition-all duration-300 hover:-translate-y-1 hover:border-[#315343]/30 hover:shadow-md dark:border-[#315343] dark:bg-[#253f33] dark:hover:border-[#C3F53C]/40">
      <div className="flex items-start justify-between gap-4">
        
        {/* Left Side: Icon & Info */}
        <div className="flex items-start gap-4">
          <div className="mt-0.5 flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#F8FAF9] text-[#75837D] transition-all duration-300 group-hover:bg-[#315343] group-hover:text-[#C3F53C] dark:bg-[#1e3329] dark:text-gray-400 dark:group-hover:bg-[#C3F53C] dark:group-hover:text-[#315343]">
            <Briefcase size={18} strokeWidth={1.75} />
          </div>
          
          <div>
            <h3 className="text-base font-bold text-[#212529] transition-colors group-hover:text-[#315343] dark:text-white dark:group-hover:text-[#C3F53C]">
              {application.jobTitle}
            </h3>
            
            <div className="mt-1 flex items-center gap-1.5 text-xs font-medium text-[#75837D] dark:text-white/60">
              <CalendarDays size={14} />
              <span>
                Applied on{" "}
                {new Date(application.submittedDate).toLocaleDateString("en-US", {
                  month: "short",
                  day: "numeric",
                  year: "numeric"
                })}
              </span>
            </div>
          </div>
        </div>

        {/* Right Side: Status Badge */}
        <div className="shrink-0">
          <ApplicationStatusBadge status={application.status} />
        </div>
        
      </div>
    </div>
  );
}