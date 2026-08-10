import { Link } from "react-router-dom";
import JobApplicationStatusBadge from "./JobApplicationStatusBadge";
import type { ApplicationWithDetails } from "../../../application/types/application";
import { getAuth } from "../../../../shared/api/authStorage";

interface JobApplicationRowProps {
  application: ApplicationWithDetails;
}

export default function JobApplicationRow({
  application,
}: JobApplicationRowProps) {
  const role = getAuth()?.role?.toLowerCase();
  return (
    <tr className="transition-colors hover:bg-[#EEF3F0]/50 dark:hover:bg-[#1E3329]/50">
      <td className="px-6 py-4">
        <Link
          to={`/hr/candidates/${application.candidateId}`}
          className="font-bold text-[#212529] transition-colors hover:text-[#315343] dark:text-white dark:hover:text-[#C3F53C]"
        >
          {application.candidateName}
        </Link>
      </td>

      <td className="px-6 py-4">
        <JobApplicationStatusBadge status={application.status} />
      </td>

      <td className="px-6 py-4 text-sm font-medium text-[#75837D] dark:text-white/80">
        {new Date(application.submittedDate).toLocaleDateString("en-US", {
          year: "numeric",
          month: "short",
          day: "numeric",
        })}
      </td>

      <td className="px-6 py-4 text-right">
      { role!== "manager" &&  (<Link
          to={`/${role}/applications/${application.id}`}
          className="inline-flex rounded-full bg-[#315343] px-6 py-1.5 text-xs font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 dark:bg-[#C3F53C] dark:text-[#315343] dark:hover:bg-[#b0df35]"
        >
          View
        </Link>)}
      </td>
    </tr>
  );
}