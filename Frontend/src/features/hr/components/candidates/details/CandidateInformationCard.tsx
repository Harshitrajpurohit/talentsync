import { Calendar, MapPin, User } from "lucide-react";
import type { User as Candidate } from "../../../../../shared/types/user";

interface CandidateInformationCardProps {
  candidate: Candidate;
}

export default function CandidateInformationCard({
  candidate,
}: CandidateInformationCardProps) {
  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
      <h2 className="mb-6 text-lg font-bold text-[#212529]">
        Personal Information
      </h2>

      <div className="space-y-5">
        <div className="flex gap-3">
          <User className="mt-0.5 text-[#315343]" size={18} />
          <div>
            <p className="text-xs font-semibold text-[#75837D] uppercase tracking-wider mb-0.5">Gender</p>
            <p className="text-sm font-medium text-[#212529]">{candidate.gender ?? "-"}</p>
          </div>
        </div>

        <div className="flex gap-3">
          <Calendar className="mt-0.5 text-[#315343]" size={18} />
          <div>
            <p className="text-xs font-semibold text-[#75837D] uppercase tracking-wider mb-0.5">Date of Birth</p>
            <p className="text-sm font-medium text-[#212529]">
              {candidate.dateOfBirth ? new Date(candidate.dateOfBirth).toLocaleDateString() : "-"}
            </p>
          </div>
        </div>

        <div className="flex gap-3">
          <MapPin className="mt-0.5 text-[#315343]" size={18} />
          <div>
            <p className="text-xs font-semibold text-[#75837D] uppercase tracking-wider mb-0.5">Address</p>
            <p className="text-sm font-medium text-[#212529]">{candidate.address ?? "-"}</p>
          </div>
        </div>

        <div className="pt-2 border-t border-[#E5EAE7]">
          <p className="mb-2 text-xs font-semibold text-[#75837D] uppercase tracking-wider">
            About
          </p>
          <p className="text-sm leading-relaxed text-[#212529]">
            {candidate.about ?? "No information provided."}
          </p>
        </div>
      </div>
    </div>
  );
}