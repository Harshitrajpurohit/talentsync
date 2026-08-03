import { Mail, Phone, FileText } from "lucide-react";
import type { ApplicationProfile } from "../../types/application";

interface CandidateInformationProps {
  application: ApplicationProfile;
}

export default function CandidateInformation({ application }: CandidateInformationProps) {
  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      <h2 className="mb-6 text-lg font-bold text-[#212529]">
        Candidate Contact
      </h2>

      <div className="space-y-6">
        <div className="flex items-center gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <Mail size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Email</p>
            <p className="text-sm font-bold text-[#212529]">{application.candidateEmail}</p>
          </div>
        </div>

        <div className="flex items-center gap-4">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <Phone size={18} />
          </div>
          <div>
            <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">Phone</p>
            <p className="text-sm font-bold text-[#212529]">{application.candidatePhone || "-"}</p>
          </div>
        </div>

        <div className="pt-2">
          {application.resumeUrl ? (
            <a
              href={application.resumeUrl}
              target="_blank"
              rel="noreferrer"
              className="inline-flex items-center gap-2 rounded-full bg-[#315343] px-5 py-2.5 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95"
            >
              <FileText size={18} />
              View Resume
            </a>
          ) : (
            <div className="inline-flex items-center gap-2 rounded-full bg-[#F8FAF9] px-5 py-2.5 text-sm font-bold text-[#75837D] border border-[#E5EAE7]">
              <FileText size={18} />
              Resume not available
            </div>
          )}
        </div>
      </div>
    </div>
  );
}