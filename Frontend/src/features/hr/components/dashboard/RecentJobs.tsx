import type { JobResponse } from "../../../jobs/types/job";
import { Link } from "react-router-dom";

type Props = {
  jobs: JobResponse[];
};

export default function RecentJobs({ jobs }: Props) {
  return (
    <div className="flex flex-col overflow-hidden rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="flex items-center justify-between border-b border-[#E5EAE7] p-5 dark:border-[#315343]">
        <h2 className="text-lg font-bold text-[#212529] dark:text-white">
          Recent Jobs
        </h2>
        <Link
          to="/hr/jobs"
          className="text-sm font-semibold text-[#75837D] transition-colors hover:text-[#315343] dark:hover:text-[#C3F53C]"
        >
          View All
        </Link>
      </div>

      <div className="overflow-x-auto">
        {jobs.length === 0 ? (
          <div className="m-6 flex h-32 items-center justify-center rounded-[16px] bg-[#F8FAF9] dark:bg-[#1e3329]">
            <p className="text-sm font-medium text-[#75837D] dark:text-white/60">
              No recent jobs posted.
            </p>
          </div>
        ) : (
          <table className="w-full">
            <thead className="bg-[#F8FAF9] dark:bg-[#1e3329]">
              <tr>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Title</th>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Department</th>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Status</th>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Posted</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-[#E5EAE7] dark:divide-[#315343]">
              {jobs.map((job) => (
                <tr key={job.id} className="group transition-colors hover:bg-[#F8FAF9]/50 dark:hover:bg-[#1e3329]/50">
                  <td className="p-4 text-sm font-bold text-[#212529] transition-colors group-hover:text-[#315343] dark:text-white dark:group-hover:text-[#C3F53C]">
                    {job.title}
                  </td>
                  <td className="p-4 text-sm font-medium text-[#75837D] dark:text-white/80">{job.department}</td>
                  <td className="p-4 text-sm font-medium text-[#75837D] dark:text-white/80">
                    <span className="rounded-full bg-[#E5EAE7]/50 px-2.5 py-1 text-[11px] font-bold uppercase tracking-wide dark:bg-[#315343]/50 dark:text-white/90">
                      {job.status}
                    </span>
                  </td>
                  <td className="p-4 text-sm font-medium text-[#75837D] dark:text-white/80">
                    {new Date(job.postedDate).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
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