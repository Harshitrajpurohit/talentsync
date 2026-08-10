import { Link } from "react-router-dom";
import type { DashboardJob } from "../../../recruiter/types/dashboard";
import { getAuth } from "../../../../shared/api/authStorage";

type Props = {
  jobs: DashboardJob[];
};

export default function RecentJobs({ jobs }: Props) {
  const role = getAuth()?.role?.toLowerCase() || "hr";

  const getBadgeStyles = (status: string) => {
    return status === "Open" 
      ? "bg-[#C3F53C]/30 text-[#315343]"
      : "bg-red-100 text-red-700";
  };

  return (
    <div className="flex flex-col overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-[#E5EAE7] p-5">
        <h2 className="text-lg font-bold text-[#212529]">
          Recent Jobs
        </h2>
        <Link
          to={`/${role}/jobs`}
          className="rounded-full px-3 py-1.5 text-xs font-bold text-[#75837D] transition-colors hover:bg-[#EEF3F0] hover:text-[#315343] active:scale-95"
        >
          View All
        </Link>
      </div>

      <div className="overflow-x-auto">
        {jobs.length === 0 ? (
          <div className="m-6 flex h-32 items-center justify-center rounded-2xl bg-[#F8FAF9]">
            <p className="text-sm font-medium text-[#75837D]">
              No recent jobs posted.
            </p>
          </div>
        ) : (
          <table className="min-w-full text-left">
            <thead className="bg-[#F8FAF9]">
              <tr>
                <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#75837D]">Title</th>
                <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#75837D]">Department</th>
                <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#75837D]">Status</th>
                <th className="px-6 py-4 text-xs font-bold uppercase tracking-wider text-[#75837D]">Posted</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-[#E5EAE7]">
              {jobs.map((job) => (
                <tr key={job.id} className="group transition-colors hover:bg-[#EEF3F0]/50">
                  <td className="px-6 py-4 text-sm font-bold text-[#212529]">
                    <Link 
                      to={`/${role}/jobs/${job.id}`}
                      className="transition-colors hover:text-[#315343] hover:underline"
                    >
                      {job.title}
                    </Link>
                  </td>
                  <td className="px-6 py-4 text-sm font-medium text-[#75837D]">
                    {job.department}
                  </td>
                  <td className="px-6 py-4">
                    <span className={`inline-flex rounded-md px-2 py-1 text-[10px] font-bold uppercase tracking-wide ${getBadgeStyles(job.status)}`}>
                      {job.status}
                    </span>
                  </td>
                  <td className="px-6 py-4 text-sm font-medium text-[#75837D]">
                    {new Date(job.postedDate).toLocaleDateString("en-US", { 
                      month: "short", 
                      day: "numeric", 
                      year: "numeric" 
                    })}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}