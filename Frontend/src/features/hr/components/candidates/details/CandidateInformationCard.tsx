import { Calendar, MapPin, User } from "lucide-react";

import type { User as Candidate } from "../../../../../shared/types/user";

interface CandidateInformationCardProps {
  candidate: Candidate;
}

export default function CandidateInformationCard({
  candidate,
}: CandidateInformationCardProps) {
  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <h2 className="mb-5 text-lg font-semibold text-[#212529] dark:text-white">
        Personal Information
      </h2>

      <div className="space-y-5">
        <div className="flex gap-3">
          <User className="mt-1 text-[#315343]" size={18} />
          <div>
            <p className="text-xs text-[#75837D]">Gender</p>
            <p>{candidate.gender ?? "-"}</p>
          </div>
        </div>

        <div className="flex gap-3">
          <Calendar className="mt-1 text-[#315343]" size={18} />
          <div>
            <p className="text-xs text-[#75837D]">Date of Birth</p>
            <p>{candidate.dateOfBirth ?? "-"}</p>
          </div>
        </div>

        <div className="flex gap-3">
          <MapPin className="mt-1 text-[#315343]" size={18} />
          <div>
            <p className="text-xs text-[#75837D]">Address</p>
            <p>{candidate.address ?? "-"}</p>
          </div>
        </div>

        <div>
          <p className="mb-2 text-xs text-[#75837D]">
            About
          </p>

          <p className="leading-relaxed text-[#495057] dark:text-gray-300">
            {candidate.about ?? "No information provided."}
          </p>
        </div>
      </div>
    </div>
  );
}