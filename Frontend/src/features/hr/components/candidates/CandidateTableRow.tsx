import { useNavigate } from "react-router-dom";
import type { User } from "../../../../shared/types/user";

interface CandidateTableRowProps {
  candidate: User;
}

export default function CandidateTableRow({ candidate }: CandidateTableRowProps) {
  const navigate = useNavigate();

  // Match the TalentSync status badge colors
  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Active":
        return "bg-[#C3F53C]/30 text-[#315343]";
      case "Inactive":
        return "bg-amber-100 text-amber-800";
      case "Suspended":
        return "bg-red-100 text-red-700";
      case "Deleted":
      default:
        return "bg-[#EEF3F0] text-[#75837D]";
    }
  };

  return (
    <tr className="transition-colors hover:bg-[#EEF3F0]/50">
      <td className="px-6 py-4">
        <div className="flex items-center gap-4 group">
          <img
            src={
              candidate.profilePictureUrl ??
              `https://ui-avatars.com/api/?background=EEF3F0&color=315343&name=${encodeURIComponent(
                candidate.name
              )}`
            }
            alt={candidate.name}
            className="h-10 w-10 shrink-0 rounded-full object-cover shadow-sm transition-transform group-hover:scale-105"
          />
          <div>
            <p className="font-bold text-[#212529] transition-colors group-hover:text-[#315343]">
              {candidate.name}
            </p>
            <p className="text-xs font-medium text-[#75837D]">
              {candidate.email}
            </p>
          </div>
        </div>
      </td>

      <td className="px-6 py-4 font-medium text-[#75837D]">
        {candidate.phone || "-"}
      </td>

      <td className="px-6 py-4">
        <span
          className={`inline-flex items-center rounded-md px-2.5 py-1 text-xs font-bold ${getStatusBadge(
            candidate.status
          )}`}
        >
          {candidate.status}
        </span>
      </td>

      <td className="px-6 py-4 text-[#75837D]">
        {new Date(candidate.createdAt).toLocaleDateString("en-US", {
          year: "numeric",
          month: "short",
          day: "numeric",
        })}
      </td>

      <td className="px-6 py-4 text-center">
        <button
          onClick={() => navigate(`/hr/candidates/${candidate.id}`)}
          className="rounded-full bg-[#315343] px-6 py-1.5 text-xs font-bold text-white transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 shadow-sm"
        >
          View
        </button>
      </td>
    </tr>
  );
}