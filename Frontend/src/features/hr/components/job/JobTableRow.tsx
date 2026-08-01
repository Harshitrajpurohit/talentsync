import JobActionMenu from "./JobActionMenu";
import JobStatusBadge from "./JobStatusBadge";
import type { JobListItem } from "../../types/job";

interface JobTableRowProps {
  job: JobListItem;
  onView: (job: JobListItem) => void;
  onEdit: (job: JobListItem) => void;
  onStatus: (job: JobListItem) => void;
}

export default function JobTableRow({
  job,
  onView,
  onEdit,
  onStatus,
}: JobTableRowProps) {
  return (
    <tr className="transition-colors hover:bg-[#EEF3F0]/50 dark:hover:bg-[#1E3329]/50">
      <td className="px-6 py-4">
        <div>
          <h3 
            className="font-bold text-[#212529] cursor-pointer transition-colors hover:text-[#315343] dark:text-white dark:hover:text-[#C3F53C]" 
            onClick={() => onView(job)}
          >
            {job.title}
          </h3>
          <p className="mt-0.5 text-xs font-medium text-[#75837D] dark:text-white/60">
            Created by {job.hrName}
          </p>
        </div>
      </td>
      <td className="px-6 py-4 font-medium text-[#75837D] dark:text-white/80">
        {job.department}
      </td>
      <td className="px-6 py-4">
        <JobStatusBadge status={job.status} />
      </td>
      <td className="px-6 py-4 font-medium text-[#212529] dark:text-white">
        {job.applicationsCount}
      </td>
      <td className="px-6 py-4 text-[#75837D] dark:text-white/80">
        {new Date(job.postedDate).toLocaleDateString("en-US", {
          year: "numeric",
          month: "short",
          day: "numeric",
        })}
      </td>
      <td className="px-6 py-4 text-right">
        <JobActionMenu job={job} onView={onView} onEdit={onEdit} onStatus={onStatus} />
      </td>
    </tr>
  );
}