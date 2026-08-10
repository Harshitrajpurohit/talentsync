import { 
  CalendarDays, 
  Clock, 
  MapPin, 
  User, 
  CalendarClock 
} from "lucide-react";

import type { CandidateInterview } from "../../types/interview";
import InterviewStatusBadge from "./InterviewStatusBadge";

import {
  formatDate,
  formatTime,
} from "../../../../shared/utils/date";

interface InterviewCardProps {
  interview: CandidateInterview;
}

export default function InterviewCard({
  interview,
}: InterviewCardProps) {
  return (
    <div className="group rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm transition-all hover:border-[#315343] hover:shadow-md">
      
      {/* Header */}
      <div className="flex items-start justify-between gap-4 border-b border-[#E5EAE7] pb-4">
        <div>
          <h2 className="text-lg font-bold text-[#212529] transition-colors group-hover:text-[#315343]">
            {interview.jobTitle}
          </h2>

          <p className="mt-1 font-mono text-xs font-medium text-[#75837D]">
            ID: {interview.id.slice(0, 8)}
          </p>
        </div>

        <InterviewStatusBadge status={interview.status} />
      </div>

      {/* Cancelled Message Banner */}
      {interview.status === "Cancelled" && (
        <div className="mt-5 flex items-start gap-3 rounded-xl border border-amber-200 bg-amber-50 p-4">
          <CalendarClock size={18} className="mt-0.5 shrink-0 text-amber-600" />
          <div>
            <p className="text-sm font-bold text-amber-900">
              Interview Cancelled
            </p>
            <p className="mt-0.5 text-xs font-medium text-amber-700">
              A new interview schedule will be shared with you soon.
            </p>
          </div>
        </div>
      )}

      {/* Details */}
      <div className="mt-5 space-y-3">
        <div className="flex items-center gap-3 text-sm font-medium text-[#75837D]">
          <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <CalendarDays size={16} />
          </div>
          <span className="text-[#212529]">
            {formatDate(interview.scheduledAt)}
          </span>
        </div>

        <div className="flex items-center gap-3 text-sm font-medium text-[#75837D]">
          <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <Clock size={16} />
          </div>
          <span className="text-[#212529]">
            {formatTime(interview.scheduledAt)}
          </span>
        </div>

        <div className="flex items-center gap-3 text-sm font-medium text-[#75837D]">
          <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <User size={16} />
          </div>
          <span className="text-[#212529]">
            {interview.interviewerName}
          </span>
        </div>

        <div className="flex items-start gap-3 text-sm font-medium text-[#75837D]">
          <div className="flex h-8 w-8 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
            <MapPin size={16} />
          </div>
          <span className="mt-1.5 leading-tight text-[#212529]">
            {interview.location || "Location will be shared later"}
          </span>
        </div>
      </div>
      
    </div>
  );
}