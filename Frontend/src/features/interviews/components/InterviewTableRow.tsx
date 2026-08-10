import { CalendarDays, MapPin } from "lucide-react";
import { Link } from "react-router-dom";
import type { InterviewDetailed } from "../types/interview";
import InterviewActionMenu from "./InterviewActionMenu";
import InterviewStatusBadge from "./InterviewStatusBadge";
import { getAuth } from "../../../shared/api/authStorage";

interface InterviewTableRowProps {
  interview: InterviewDetailed;
  onRecordOutcome: (interview: InterviewDetailed) => void;
}

export default function InterviewTableRow({
  interview,
  onRecordOutcome,
}: InterviewTableRowProps) {
  const scheduledDate = new Date(interview.scheduledAt);
  const role = getAuth()?.role?.toLowerCase() || "hr";

  return (
    <tr className="transition-colors hover:bg-[#EEF3F0]/50">
      {/* Candidate */}
      <td className="px-6 py-4">
        <div>
          <Link
            to={`/${role}/candidates/${interview.candidateId}`}
            className="font-bold text-[#212529] transition-colors hover:text-[#315343]"
          >
            {interview.candidateName}
          </Link>
          <p className="mt-0.5 text-xs font-medium text-[#75837D]">
            {interview.candidateEmail}
          </p>
        </div>
      </td>

      {/* Job */}
      <td className="px-6 py-4">
        <Link
          to={`/${role}/jobs/${interview.jobId}`}
          className="font-bold text-[#315343] transition-colors hover:text-[#38805e]"
        >
          <span className="block max-w-[200px] truncate">{interview.jobTitle}</span>
        </Link>
      </td>

      {/* Scheduled */}
      <td className="px-6 py-4">
        <div className="flex items-center gap-3 text-sm font-medium text-[#75837D]">
          <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <CalendarDays size={16} />
          </div>
          <div>
            <p className="font-bold text-[#212529]">
              {scheduledDate.toLocaleDateString("en-US", {
                day: "2-digit",
                month: "short",
                year: "numeric",
              })}
            </p>
            <p className="text-xs">
              {scheduledDate.toLocaleTimeString("en-US", {
                hour: "2-digit",
                minute: "2-digit",
              })}
            </p>
          </div>
        </div>
      </td>

      {/* Location */}
      <td className="px-6 py-4">
        <div className="flex max-w-[180px] items-center gap-2">
          <MapPin size={16} className="shrink-0 text-[#75837D]" />
          <span className="truncate font-medium text-[#75837D]">
            {interview.location || "Not specified"}
          </span>
        </div>
      </td>

      {/* Status */}
      <td className="px-6 py-4">
        <InterviewStatusBadge status={interview.status} />
      </td>

      {/* Action */}
      <td className="px-6 py-4 text-right">
        <InterviewActionMenu
          status={interview.status}
          onRecordOutcome={() => onRecordOutcome(interview)}
        />
      </td>
    </tr>
  );
}