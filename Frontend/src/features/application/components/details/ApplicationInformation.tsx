import { Calendar, Clock3, FileText, BadgeCheck } from "lucide-react";
import type { ApplicationProfile } from "../../types/application";

interface ApplicationInformationProps {
  application: ApplicationProfile;
}

function formatDate(date: string) {
  return new Date(date).toLocaleString("en-US", {
    dateStyle: "medium",
    timeStyle: "short",
  });
}

export default function ApplicationInformation({ application }: ApplicationInformationProps) {
  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      <h2 className="mb-6 text-lg font-bold text-[#212529]">
        Application Information
      </h2>

      <div className="grid gap-6 md:grid-cols-2">
        <div className="flex gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <FileText size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Application ID</p>
            <p className="break-all text-sm font-bold text-[#212529]">{application.id}</p>
          </div>
        </div>

        <div className="flex gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <BadgeCheck size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Current Status</p>
            <p className="text-sm font-bold text-[#212529]">{application.status}</p>
          </div>
        </div>

        <div className="flex gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <Calendar size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Submitted</p>
            <p className="text-sm font-bold text-[#212529]">{formatDate(application.submittedDate)}</p>
          </div>
        </div>

        <div className="flex gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <Clock3 size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Created</p>
            <p className="text-sm font-bold text-[#212529]">{formatDate(application.createdAt)}</p>
          </div>
        </div>
      </div>
    </div>
  );
}