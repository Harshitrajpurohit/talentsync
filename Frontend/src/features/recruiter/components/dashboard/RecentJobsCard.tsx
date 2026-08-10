import { Link } from "react-router-dom";
import { ArrowRight } from "lucide-react";

import JobStatusBadge from "../../../jobs/components/JobStatusBadge";

import { getAuth } from "../../../../shared/api/authStorage";

import type { DashboardJob } from "../../types/dashboard";

interface RecentJobsCardProps {
  jobs: DashboardJob[];
}

export default function RecentJobsCard({
  jobs,
}: RecentJobsCardProps) {
  const role = getAuth()?.role?.toLowerCase();

  return (
    <div className="rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-[#E5EAE7] px-6 py-5">
        <div>
          <h2 className="text-lg font-bold text-[#212529]">
            Recent Jobs
          </h2>

          <p className="text-sm text-[#75837D]">
            Recently created job postings.
          </p>
        </div>
      </div>

      <div className="divide-y divide-[#E5EAE7]">
        {jobs.length === 0 ? (
          <div className="py-10 text-center text-sm text-[#75837D]">
            No recent jobs.
          </div>
        ) : (
          jobs.map((job) => (
            <div
              key={job.id}
              className="flex items-center justify-between px-6 py-4"
            >
              <div>
                <Link
                  to={`/${role}/jobs/${job.id}`}
                  className="font-bold text-[#212529] hover:text-[#315343]"
                >
                  {job.title}
                </Link>

                <p className="mt-1 text-sm text-[#75837D]">
                  {job.department}
                </p>

                <p className="mt-1 text-xs text-[#75837D]">
                  {new Date(job.postedDate).toLocaleDateString()}
                </p>
              </div>

              <div className="flex items-center gap-3">
                <JobStatusBadge status={job.status} />

                <Link
                  to={`/${role}/jobs/${job.id}`}
                  className="rounded-full border border-[#E5EAE7] p-2 transition hover:bg-[#EEF3F0]"
                >
                  <ArrowRight size={16} />
                </Link>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}