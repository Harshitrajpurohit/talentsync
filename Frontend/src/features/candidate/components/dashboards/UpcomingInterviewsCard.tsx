import { CalendarClock, MapPin, User, MoreHorizontal } from "lucide-react";
import { Link } from "react-router-dom";
import type { DashboardInterview } from "../../types/dashboard";

interface UpcomingInterviewsCardProps {
  interviews: DashboardInterview[];
  viewAllLink?: string;
}

export default function UpcomingInterviewsCard({
  interviews,
  viewAllLink = "/candidate/interviews",
}: UpcomingInterviewsCardProps) {
  return (
    <div className="flex h-full flex-col rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="mb-5 flex items-center justify-between">
        <h2 className="text-lg font-bold text-[#212529] dark:text-white">
          Upcoming Interviews
        </h2>
        <Link
          to={viewAllLink}
          className="flex items-center gap-1 text-sm font-semibold text-[#75837D] transition-colors hover:text-[#315343] dark:hover:text-[#C3F53C]"
        >
          View All
        </Link>
      </div>

      <div className="flex-1 space-y-3">
        {interviews.length === 0 ? (
          <div className="flex h-28 items-center justify-center rounded-xl bg-[#F8FAF9] dark:bg-[#1e3329]">
            <p className="text-sm font-medium text-[#75837D]">No interviews scheduled.</p>
          </div>
        ) : (
          interviews.map((interview) => (
            <div
              key={interview.id}
              className="group flex items-start justify-between gap-3 rounded-xl p-2 transition-colors hover:bg-[#F8FAF9] dark:hover:bg-[#1e3329] sm:items-center"
            >
              <div className="flex items-start gap-3 sm:items-center">
                <div className="mt-1 flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#C3F53C]/20 text-[#315343] dark:bg-[#C3F53C]/10 dark:text-[#C3F53C] sm:mt-0">
                  <CalendarClock size={18} />
                </div>
                <div>
                  <h3 className="font-bold text-[#212529] dark:text-white">
                    {interview.jobTitle}
                  </h3>
                  <div className="mt-1 flex flex-wrap items-center gap-x-3 gap-y-1 text-xs font-medium text-[#75837D]">
                    <span className="flex items-center gap-1.5">
                      <User size={12} />
                      {interview.interviewerName}
                    </span>
                    <span className="hidden h-1 w-1 rounded-full bg-[#E5EAE7] dark:bg-gray-600 sm:block" />
                    <span className="flex items-center gap-1.5">
                      {new Date(interview.scheduledAt).toLocaleString("en-US", {
                        month: "short",
                        day: "numeric",
                        hour: "numeric",
                        minute: "2-digit",
                      })}
                    </span>
                    {interview.location && (
                      <>
                        <span className="hidden h-1 w-1 rounded-full bg-[#E5EAE7] dark:bg-gray-600 sm:block" />
                        <span className="flex items-center gap-1.5">
                          <MapPin size={12} />
                          {interview.location}
                        </span>
                      </>
                    )}
                  </div>
                </div>
              </div>

              <button className="text-[#75837D] transition-colors hover:text-[#212529] dark:hover:text-white">
                <MoreHorizontal size={18} />
              </button>
            </div>
          ))
        )}
      </div>
    </div>
  );
}