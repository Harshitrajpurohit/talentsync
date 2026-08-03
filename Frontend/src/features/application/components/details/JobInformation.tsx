import { BriefcaseBusiness, Building2 } from "lucide-react";
import type { ApplicationProfile } from "../../types/application";

interface JobInformationProps {
  application: ApplicationProfile;
}

export default function JobInformation({ application }: JobInformationProps) {
  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      <h2 className="mb-6 text-lg font-bold text-[#212529]">
        Job Details
      </h2>

      <div className="space-y-6">
        <div className="flex items-center gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <BriefcaseBusiness size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Job Title</p>
            <p className="text-sm font-bold text-[#212529]">{application.jobTitle}</p>
          </div>
        </div>

        <div className="flex items-center gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <Building2 size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Department</p>
            <p className="text-sm font-bold text-[#212529]">{application.department}</p>
          </div>
        </div>
      </div>
    </div>
  );
}