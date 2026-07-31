import { useNavigate } from "react-router-dom";
import type { User } from "../../../../shared/types/user";

interface CandidateTableRowProps {
  candidate: User;
}

export default function CandidateTableRow({
  candidate,
}: CandidateTableRowProps) {
  const navigate = useNavigate();

  // Match the soft green badge in the image
  const getStatusBadge = (status: string) => {
    switch (status) {
      case "Active":
        return "bg-[#e8f7ed] text-[#2c7a4b] dark:bg-[#2c7a4b]/20 dark:text-[#6ee7b7]";
      case "Inactive":
        return "bg-amber-50 text-amber-700 dark:bg-amber-900/30 dark:text-amber-300";
      case "Suspended":
        return "bg-rose-50 text-rose-700 dark:bg-rose-900/30 dark:text-rose-300";
      case "Deleted":
      default:
        return "bg-gray-50 text-gray-500 dark:bg-[#1e3329] dark:text-gray-400";
    }
  };

  return (
    <tr className="border-t border-gray-100 transition-colors hover:bg-gray-50/50 dark:border-[#315343] dark:hover:bg-[#1e3329]/50">
      <td className="px-6 py-4">
        <div className="flex items-center gap-4">
          <img
            src={
              candidate.profilePictureUrl ??
              `https://ui-avatars.com/api/?name=${encodeURIComponent(candidate.name)}&background=F8FAF9&color=315343`
            }
            alt={candidate.name}
            className="h-9 w-9 rounded-full object-cover"
          />
          <div>
            <p className="text-sm font-medium text-gray-900 dark:text-white">
              {candidate.name}
            </p>
            <p className="mt-0.5 text-[13px] text-gray-500 dark:text-white/60">
              {candidate.email}
            </p>
          </div>
        </div>
      </td>

      <td className="px-6 py-4 text-[13px] text-gray-500 dark:text-white/80">
        {candidate.phone || "-"}
      </td>

      <td className="px-6 py-4">
        <span
          className={`inline-flex items-center rounded-full px-3 py-1 text-[12px] font-medium ${getStatusBadge(
            candidate.status
          )}`}
        >
          {candidate.status}
        </span>
      </td>

      <td className="px-6 py-4 text-[13px] text-gray-500 dark:text-white/80">
        {new Date(candidate.createdAt).toLocaleDateString("en-US")}
      </td>

      <td className="px-6 py-4 text-center">
        {/* Exactly matching the dark green pill button from the image */}
        <button
          onClick={() => navigate(`/hr/candidates/${candidate.id}`)}
          className="rounded-full bg-[#315343] px-6 py-1.5 text-[13px] font-semibold text-white transition-all hover:bg-[#C3F53C] hover:text-black dark:bg-[#C3F53C] dark:text-[#315343] dark:hover:bg-[#b0df35]"
        >
          View
        </button>
      </td>
    </tr>
  );
}