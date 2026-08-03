import { Briefcase, CalendarDays, User } from "lucide-react";
import ApplicationStatusBadge from "../ApplicationStatusBadge";
import type { ApplicationProfile } from "../../types/application";

interface ApplicationHeaderProps {
  application: ApplicationProfile;
}

function formatDate(date: string) {
  return new Date(date).toLocaleDateString("en-US", {
    day: "2-digit",
    month: "short",
    year: "numeric",
  });
}

export default function ApplicationHeader({ application }: ApplicationHeaderProps) {
  return (
    <div className="relative overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      {/* Decorative top border accent */}
      <div className="absolute left-0 top-0 h-1.5 w-full bg-[#315343]">
        <div className="h-full w-1/4 bg-[#C3F53C]"></div>
      </div>

      <div className="flex flex-col gap-5 lg:flex-row lg:items-center lg:justify-between">
        <div className="space-y-3">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
              <User size={20} />
            </div>
            <h1 className="text-2xl font-bold text-[#212529]">
              {application.candidateName}
            </h1>
          </div>

          <div className="flex flex-wrap items-center gap-x-5 gap-y-2 text-sm font-medium text-[#75837D]">
            <div className="flex items-center gap-2">
              <Briefcase size={16} className="text-[#315343]" />
              <span className="text-[#212529]">{application.jobTitle}</span>
            </div>
            
            <div className="hidden h-1 w-1 rounded-full bg-[#E5EAE7] sm:block" />

            <div className="flex items-center gap-2">
              <CalendarDays size={16} className="text-[#315343]" />
              <span>Applied {formatDate(application.submittedDate)}</span>
            </div>
          </div>
        </div>

        <div className="shrink-0">
          <ApplicationStatusBadge status={application.status} />
        </div>
      </div>
    </div>
  );
}