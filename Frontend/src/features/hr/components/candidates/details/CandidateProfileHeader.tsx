import { CalendarDays, Mail, Phone, UserCircle2 } from "lucide-react";
import type { User } from "../../../../../shared/types/user";

interface CandidateProfileHeaderProps {
  candidate: User;
}

export default function CandidateProfileHeader({
  candidate,
}: CandidateProfileHeaderProps) {
  
  // Adjusted for dark background visibility
  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Active":
        return "bg-[#C3F53C] text-[#315343]";
      case "Inactive":
        return "bg-amber-400 text-amber-950";
      case "Suspended":
        return "bg-red-400 text-red-950";
      case "Deleted":
      default:
        return "bg-white/20 text-white";
    }
  };

  return (
    <div className="overflow-hidden rounded-2xl bg-[#315343] shadow-sm">
      <div className="flex flex-col gap-6 p-6 md:flex-row md:items-center md:justify-between">
        <div className="flex items-center gap-5">
          {candidate.profilePictureUrl ? (
            <img
              src={candidate.profilePictureUrl}
              alt={candidate.name}
              className="h-24 w-24 rounded-full border-4 border-[#C3F53C] object-cover shadow-md"
            />
          ) : (
            <div className="flex h-24 w-24 items-center justify-center rounded-full bg-white/10 border-2 border-dashed border-[#C3F53C]/50">
              <UserCircle2 className="h-12 w-12 text-[#C3F53C]" />
            </div>
          )}

          <div>
            <h1 className="text-3xl font-bold text-white tracking-tight">
              {candidate.name}
            </h1>

            <div className="mt-3 flex flex-wrap gap-2">
              <span
                className={`rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${getStatusBadge(
                  candidate.status
                )}`}
              >
                {candidate.status}
              </span>
            </div>
          </div>
        </div>

        <div className="space-y-3 text-sm font-medium text-[#E5EAE7]">
          <div className="flex items-center gap-3">
            <Mail size={16} className="text-[#C3F53C]" />
            {candidate.email}
          </div>

          <div className="flex items-center gap-3">
            <Phone size={16} className="text-[#C3F53C]" />
            {candidate.phone ?? "Not provided"}
          </div>

          <div className="flex items-center gap-3">
            <CalendarDays size={16} className="text-[#C3F53C]" />
            Joined{" "}
            {new Date(candidate.createdAt).toLocaleDateString()}
          </div>
        </div>
      </div>
    </div>
  );
}