import JobTableRow from "./JobTableRow";
import type { JobListItem } from "../../types/job";

interface JobTableProps {
  jobs: JobListItem[];
  onView: (job: JobListItem) => void;
  onEdit: (job: JobListItem) => void;
  onStatus: (job: JobListItem) => void;
}

export default function JobTable({
  jobs,
  onView,
  onEdit,
  onStatus,
}: JobTableProps) {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="overflow-x-auto">
        <table className="min-w-full text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9] dark:border-[#315343] dark:bg-[#1E3329]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Job</th>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Department</th>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Status</th>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Applications</th>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Posted</th>
              <th className="px-6 py-4 text-right font-semibold text-[#75837D] dark:text-white/80">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7] dark:divide-[#315343]">
            {jobs.map((job) => (
              <JobTableRow
                key={job.id}
                job={job}
                onView={onView}
                onEdit={onEdit}
                onStatus={onStatus}
              />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}