import { Link } from "react-router-dom";
import { MoreHorizontal, Briefcase } from "lucide-react";
import type { DashboardApplication } from "../../types/dashboard";

interface RecentApplicationsCardProps {
  applications: DashboardApplication[];
  viewAllLink?: string;
}

function getStatusBadge(status: DashboardApplication["status"]) {
  switch (status) {
    case "Selected":
      return "bg-[#C3F53C]/20 text-[#315343] dark:bg-[#C3F53C]/10 dark:text-[#C3F53C]";
    case "InterviewScheduled":
    case "InterviewCompleted":
      return "bg-[#315343]/10 text-[#315343] dark:bg-[#315343]/50 dark:text-white";
    case "Rejected":
      return "bg-rose-50 text-rose-700 dark:bg-rose-900/30 dark:text-rose-300";
    case "Screening":
      return "bg-amber-50 text-amber-700 dark:bg-amber-900/30 dark:text-amber-300";
    case "Submitted":
    default:
      return "bg-[#F8FAF9] text-[#75837D] border border-[#E5EAE7] dark:bg-[#1e3329] dark:text-white/70 dark:border-[#315343]";
  }
}

export default function RecentApplicationsCard({
  applications,
  viewAllLink = "/candidate/applications",
}: RecentApplicationsCardProps) {
  return (
    <div className="flex h-full flex-col rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="mb-5 flex items-center justify-between">
        <h2 className="text-lg font-bold text-[#212529] dark:text-white">
          Recent Applications
        </h2>
        <Link
          to={viewAllLink}
          className="flex items-center gap-1 text-sm font-semibold text-[#75837D] transition-colors hover:text-[#315343] dark:hover:text-[#C3F53C]"
        >
          View All
        </Link>
      </div>

      <div className="flex-1 space-y-3">
        {applications.length === 0 ? (
          <div className="flex h-28 items-center justify-center rounded-xl bg-[#F8FAF9] dark:bg-[#1e3329]">
            <p className="text-sm font-medium text-[#75837D]">No recent applications.</p>
          </div>
        ) : (
          applications.map((app) => (
            <div
              key={app.id}
              className="group flex items-center justify-between gap-3 rounded-xl p-2 transition-colors hover:bg-[#F8FAF9] dark:hover:bg-[#1e3329]"
            >
              <div className="flex items-center gap-3">
                <div className="flex h-10 w-10 items-center justify-center rounded-full bg-[#F8FAF9] text-[#75837D] dark:bg-[#253f33] dark:text-gray-400">
                  <Briefcase size={18} />
                </div>
                <div>
                  <h3 className="font-bold text-[#212529] dark:text-white">
                    {app.jobTitle}
                  </h3>
                  <p className="mt-0.5 flex items-center gap-1.5 text-xs font-medium text-[#75837D]">
                    ID: #{app.jobId}
                    <span className="h-1 w-1 rounded-full bg-[#E5EAE7] dark:bg-gray-600" />
                    {new Date(app.submittedDate).toLocaleDateString("en-US", {
                      month: "short",
                      day: "numeric",
                    })}
                  </p>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <span
                  className={`hidden rounded-full px-2.5 py-1 text-[11px] font-bold uppercase tracking-wide sm:block ${getStatusBadge(
                    app.status
                  )}`}
                >
                  {app.status.replace(/([A-Z])/g, " $1").trim()}
                </span>
                <button className="text-[#75837D] transition-colors hover:text-[#212529] dark:hover:text-white">
                  <MoreHorizontal size={18} />
                </button>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}