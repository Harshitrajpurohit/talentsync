import JobStatusBadge from "../JobStatusBadge";
import type { JobDetails } from "../../types/job";

interface JobHeaderProps {
  job: JobDetails;
}

export default function JobHeader({ job }: JobHeaderProps) {
  return (
    <div className="relative overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      {/* Decorative top border accent */}
      <div className="absolute left-0 top-0 h-1.5 w-full bg-[#315343]">
        <div className="h-full w-1/4 bg-[#C3F53C]"></div>
      </div>

      <div className="flex flex-col gap-5 lg:flex-row lg:items-start lg:justify-between">
        <div>
          <h1 className="text-2xl font-bold text-[#212529] sm:text-3xl">
            {job.title}
          </h1>

          <div className="mt-3 flex flex-wrap items-center gap-x-3 gap-y-2 text-sm font-medium text-[#75837D]">
            <span className="flex items-center gap-1.5 text-[#212529]">
              <div className="h-2 w-2 rounded-full bg-[#C3F53C]"></div>
              {job.department}
            </span>

            <span className="text-[#E5EAE7]">•</span>

            <span>
              Posted {new Date(job.postedDate).toLocaleDateString()}
            </span>

            <span className="text-[#E5EAE7]">•</span>

            <span>Created By: <span className="text-[#212529]">{job.hrName}</span></span>
          </div>
        </div>

        <div className="shrink-0">
          <JobStatusBadge status={job.status} />
        </div>
      </div>
    </div>
  );
}