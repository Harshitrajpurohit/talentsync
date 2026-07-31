import { CalendarDays, Mail, Phone, UserCircle2 } from "lucide-react";

import type { User } from "../../../../../shared/types/user";

interface CandidateProfileHeaderProps {
  candidate: User;
}

export default function CandidateProfileHeader({
  candidate,
}: CandidateProfileHeaderProps) {
  const statusColor = {
    Active: "bg-green-100 text-green-700",
    Inactive: "bg-yellow-100 text-yellow-700",
    Suspended: "bg-red-100 text-red-700",
    Deleted: "bg-gray-100 text-gray-700",
  };

  return (
    <div className="overflow-hidden rounded-[20px] bg-[#315343] shadow-sm">
      <div className="flex flex-col gap-6 p-6 md:flex-row md:items-center md:justify-between">
        <div className="flex items-center gap-5">
          {candidate.profilePictureUrl ? (
            <img
              src={candidate.profilePictureUrl}
              alt={candidate.name}
              className="h-24 w-24 rounded-full border-4 border-[#C3F53C] object-cover"
            />
          ) : (
            <div className="flex h-24 w-24 items-center justify-center rounded-full bg-white/10">
              <UserCircle2 className="h-16 w-16 text-[#C3F53C]" />
            </div>
          )}

          <div>
            <h1 className="text-3xl font-bold text-white">
              {candidate.name}
            </h1>

            <div className="mt-2 flex flex-wrap gap-2">
              <span
                className={`rounded-full px-3 py-1 text-xs font-semibold ${
                  statusColor[candidate.status]
                }`}
              >
                {candidate.status}
              </span>
            </div>
          </div>
        </div>

        <div className="space-y-3 text-sm text-[#E5EAE7]">
          <div className="flex items-center gap-2">
            <Mail size={16} className="text-[#C3F53C]" />
            {candidate.email}
          </div>

          <div className="flex items-center gap-2">
            <Phone size={16} className="text-[#C3F53C]" />
            {candidate.phone ?? "-"}
          </div>

          <div className="flex items-center gap-2">
            <CalendarDays size={16} className="text-[#C3F53C]" />
            Joined{" "}
            {new Date(candidate.createdAt).toLocaleDateString()}
          </div>
        </div>
      </div>
    </div>
  );
}