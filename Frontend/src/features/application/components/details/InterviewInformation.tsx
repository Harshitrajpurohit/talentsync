import {
  CalendarDays,
  MapPin,
  User,
  MessageSquare,
  CalendarX2,
} from "lucide-react";

import type { Interview } from "../../../interviews/types/interview";

interface InterviewInformationProps {
  interview: Interview | null;
}

const statusColors: Record<string, string> = {
  Pending: "bg-amber-100 text-amber-800",
  Scheduled: "bg-blue-100 text-blue-700",
  Completed: "bg-indigo-100 text-indigo-700",
  Passed: "bg-[#C3F53C]/30 text-[#315343]",
  Failed: "bg-red-100 text-red-700",
  Cancelled: "bg-[#EEF3F0] text-[#75837D]",
};

export default function InterviewInformation({
  interview,
}: InterviewInformationProps) {
  return (
    <section className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      <div className="mb-6 flex items-center justify-between">
        <h2 className="text-lg font-bold text-[#212529]">
          Interview Information
        </h2>

        {interview && (
          <span
            className={`inline-flex rounded-md px-2.5 py-1 text-xs font-bold uppercase tracking-wider ${
              statusColors[interview.status] || "bg-[#EEF3F0] text-[#75837D]"
            }`}
          >
            {interview.status}
          </span>
        )}
      </div>

      {!interview ? (
        <div className="flex flex-col items-center justify-center rounded-xl border-2 border-dashed border-[#E5EAE7] bg-[#F8FAF9] py-12 text-center">
          <CalendarX2 size={28} className="mb-3 text-[#75837D]" />
          <p className="text-sm font-bold text-[#212529]">
            No Interview Scheduled
          </p>
          <p className="mt-1 text-xs font-medium text-[#75837D]">
            An interview has not been set up for this application yet.
          </p>
        </div>
      ) : (
        <div className="space-y-6">
          <div className="grid gap-6 md:grid-cols-2">
            
            {/* Scheduled At */}
            <div className="flex items-start gap-4">
              <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
                <CalendarDays size={18} />
              </div>
              <div>
                <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">
                  Scheduled At
                </p>
                <p className="text-sm font-bold text-[#212529]">
                  {new Date(interview.scheduledAt).toLocaleString("en-US", {
                    weekday: "short",
                    year: "numeric",
                    month: "short",
                    day: "numeric",
                    hour: "2-digit",
                    minute: "2-digit",
                  })}
                </p>
              </div>
            </div>

            {/* Interviewer */}
            <div className="flex items-start gap-4">
              <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
                <User size={18} />
              </div>
              <div>
                <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">
                  Interviewer
                </p>
                <p className="text-sm font-bold text-[#212529]">
                  {interview.interviewerName}
                </p>
              </div>
            </div>

            {/* Location */}
            <div className="flex items-start gap-4 md:col-span-2">
              <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
                <MapPin size={18} />
              </div>
              <div>
                <p className="mb-0.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">
                  Location / Link
                </p>
                <p className="text-sm font-bold text-[#212529]">
                  {interview.location || "Not specified"}
                </p>
              </div>
            </div>
          </div>

          {/* Feedback Section */}
          {interview.feedback && (
            <div className="border-t border-[#E5EAE7] pt-6">
              <div className="flex items-start gap-4">
                <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
                  <MessageSquare size={18} />
                </div>
                <div className="w-full">
                  <p className="mb-1.5 text-xs font-semibold uppercase tracking-wider text-[#75837D]">
                    Feedback & Notes
                  </p>
                  <div className="rounded-xl border border-[#E5EAE7] bg-[#F8FAF9] p-4 text-sm font-medium leading-relaxed text-[#212529]">
                    {interview.feedback}
                  </div>
                </div>
              </div>
            </div>
          )}
          
        </div>
      )}
    </section>
  );
}