import { Link, useNavigate } from "react-router-dom";
import ApplicationStatusBadge from "./ApplicationStatusBadge";
import ApplicationActionMenu from "./ApplicationActionMenu";
import type { ApplicationWithDetails } from "../../application/types/application";
import { getAuth } from "../../../shared/api/authStorage";

interface ApplicationTableRowProps {
  application: ApplicationWithDetails;
  onScreening: (application: ApplicationWithDetails) => void;
  onInterview: (application: ApplicationWithDetails) => void;
  onSelection: (application: ApplicationWithDetails) => void;
}

export default function ApplicationTableRow({
  application,
  onScreening,
  onInterview,
  onSelection,
}: ApplicationTableRowProps) {
  const navigate = useNavigate();
  const role = getAuth()?.role?.toLowerCase();
  return (
    <tr className="transition-colors hover:bg-[#EEF3F0]/50">
      <td className="px-6 py-4">
        <Link 
          to={`/hr/candidates/${application.candidateId}`}
          className="font-bold text-[#212529] transition-colors hover:text-[#315343]"
        >
          {application.candidateName}
        </Link>
      </td>

      <td className="px-6 py-4">
        <Link 
          to={`/hr/jobs/${application.jobId}`}
          className="font-medium text-[#75837D] transition-colors hover:text-[#212529]"
        >
          {application.jobTitle}
        </Link>
      </td>

      <td className="px-6 py-4 font-medium text-[#75837D]">
        {new Date(application.submittedDate).toLocaleDateString("en-US", {
          year: "numeric",
          month: "short",
          day: "numeric",
        })}
      </td>

      <td className="px-6 py-4">
        <ApplicationStatusBadge status={application.status} />
      </td>

      <td className="px-6 py-4 text-right">
        <ApplicationActionMenu
          status={application.status}
          onView={() => navigate(`/${role}/applications/${application.id}`)}
          onScreening={() => onScreening(application)}
          onInterview={() => onInterview(application)}
          onSelection={() => onSelection(application)}
        />
      </td>
    </tr>
  );
}