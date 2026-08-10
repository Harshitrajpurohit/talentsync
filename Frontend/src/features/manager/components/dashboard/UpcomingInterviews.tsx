import { CalendarDays, Clock, MapPin, User } from "lucide-react";
import { Link } from "react-router-dom";
import type { InterviewDetailed } from "../../../interviews/types/interview";
import { getAuth } from "../../../../shared/api/authStorage";

interface UpcomingInterviewsProps {
  interviews: InterviewDetailed[];
}

function formatInterviewDate(value: string) {
  return new Date(value).toLocaleDateString("en-US", {
    weekday: "short",
    month: "short",
    day: "numeric",
  });
}

function formatInterviewTime(value: string) {
  return new Date(value).toLocaleTimeString("en-US", {
    hour: "numeric",
    minute: "2-digit",
  });
}

export default function UpcomingInterviews({
  interviews,
}: UpcomingInterviewsProps) {
  const role = getAuth()?.role?.toLowerCase() || "hr";

  return (
    <div className="flex flex-col overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      {/* Header */}
      <div className="flex items-center justify-between border-b border-[#E5EAE7] p-5">
        <div>
          <h2 className="text-lg font-bold text-[#212529]">
            Upcoming Interviews
          </h2>
        </div>

        <Link
          to={`/${role}/interviews`}
          className="shrink-0 rounded-full px-3 py-1.5 text-xs font-bold text-[#75837D] transition-colors hover:bg-[#EEF3F0] hover:text-[#315343] active:scale-95"
        >
          View All
        </Link>
      </div>

      {/* Content */}
      <div className="p-5">
        {!interviews.length ? (
          <div className="flex min-h-[220px] flex-col items-center justify-center rounded-2xl border border-dashed border-[#E5EAE7] bg-[#F8FAF9] text-center">
            <div className="mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
              <CalendarDays size={20} />
            </div>
            <p className="text-sm font-bold text-[#212529]">
              No upcoming interviews
            </p>
            <p className="mt-1 max-w-[200px] text-xs font-medium text-[#75837D]">
              You currently have no scheduled interviews coming up.
            </p>
          </div>
        ) : (
          <div className="flex flex-col gap-4">
            {interviews.map((interview) => (
              <div
                key={interview.id}
                className="group flex flex-col gap-3 rounded-xl border border-[#E5EAE7] bg-white p-4 transition-all hover:border-[#315343] hover:shadow-sm sm:flex-row sm:items-start"
              >
                <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
                  <User size={18} />
                </div>

                <div className="min-w-0 flex-1">
                  <h3 className="truncate text-sm font-bold text-[#212529] transition-colors group-hover:text-[#315343]">
                    {interview.candidateName}
                  </h3>
                  <p className="truncate text-xs font-medium text-[#75837D]">
                    {interview.jobTitle}
                  </p>

                  <div className="mt-3 flex flex-wrap items-center gap-x-4 gap-y-2">
                    <div className="flex items-center gap-1.5 text-xs font-medium text-[#75837D]">
                      <CalendarDays size={14} className="text-[#315343]" />
                      <span>{formatInterviewDate(interview.scheduledAt)}</span>
                    </div>

                    <div className="flex items-center gap-1.5 text-xs font-medium text-[#75837D]">
                      <Clock size={14} className="text-[#315343]" />
                      <span>{formatInterviewTime(interview.scheduledAt)}</span>
                    </div>

                    {interview.location && (
                      <div className="flex items-center gap-1.5 text-xs font-medium text-[#75837D]">
                        <MapPin size={14} className="shrink-0 text-[#315343]" />
                        <span className="truncate max-w-[120px]">
                          {interview.location}
                        </span>
                      </div>
                    )}
                  </div>
                </div>
              </div>
            ))}
          </div>
        )}
      </div>
    </div>
  );
}