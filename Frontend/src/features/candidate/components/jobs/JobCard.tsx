import { Briefcase, CalendarDays } from "lucide-react";
import type { CandidateJob } from "../../types/job";
import JobStatusBadge from "./JobStatusBadge";

interface JobCardProps {
  job: CandidateJob;
  onView: (id: string) => void;
}

export default function JobCard({
  job,
  onView,
}: JobCardProps) {
  return (
    <div className="group rounded-[20px] border border-[#E5EAE7] bg-white p-5 shadow-sm transition-all duration-300 hover:border-[#315343] hover:shadow-md dark:border-gray-700 dark:bg-gray-900">
      <div className="flex items-start justify-between gap-3">
        <div>
          <h3 className="text-lg font-semibold text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
            {job.title}
          </h3>

          <div className="mt-2 flex items-center gap-2 text-sm text-[#75837D] transition-colors duration-300">
            <Briefcase 
              size={16} 
              className="transition-colors duration-300 group-hover:text-[#C3F53C]" 
            />
            <span className="transition-colors duration-300 group-hover:text-[#315343]">
              {job.department}
            </span>
          </div>

          <div className="mt-2 flex items-center gap-2 text-sm text-[#75837D] transition-colors duration-300">
            <CalendarDays 
              size={16} 
              className="transition-colors duration-300 group-hover:text-[#C3F53C]" 
            />
            <span className="transition-colors duration-300 group-hover:text-[#315343]">
              {new Date(job.postedDate).toLocaleDateString()}
            </span>
          </div>
        </div>

        <JobStatusBadge status={job.status} />
      </div>

      <div className="mt-5 flex items-center justify-between">
        <span
          className={`text-sm font-medium ${
            job.hasApplied ? "text-[#315343]" : "text-[#75837D]"
          }`}
        >
          {job.hasApplied ? "Applied" : "Not Applied"}
        </span>

        <button
          onClick={() => onView(job.id)}
          className="rounded-[20px] bg-[#315343] px-4 py-2 text-sm font-semibold text-white shadow-sm transition-all duration-300 hover:bg-[#C3F53C] hover:text-[#315343] hover:shadow-md"
        >
          View Details
        </button>
      </div>
    </div>
  );
}