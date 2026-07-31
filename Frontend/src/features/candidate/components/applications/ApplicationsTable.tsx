import type { ApplicationWithDetails } from "../../../application/types/application";
import ApplicationStatusBadge from "./ApplicationStatusBadge";

interface ApplicationsTableProps {
  applications: ApplicationWithDetails[];
}

export default function ApplicationsTable({
  applications,
}: ApplicationsTableProps) {
  return (
    <div className="overflow-hidden rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <div className="overflow-x-auto">
        <table className="min-w-full">
          <thead className="bg-[#E5EAE7]/20 dark:bg-gray-800">
            <tr>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Job Title
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Applied On
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Status
              </th>
            </tr>
          </thead>

          <tbody>
            {applications.map((application) => (
              <tr
                key={application.id}
                className="group border-t border-[#E5EAE7] transition-colors duration-300 hover:bg-[#E5EAE7]/10 dark:border-gray-700 dark:hover:bg-gray-800/50"
              >
                <td className="px-6 py-4 font-medium text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
                  {application.jobTitle}
                </td>

                <td className="px-6 py-4 text-sm text-[#75837D] dark:text-gray-400">
                  {new Date(application.submittedDate).toLocaleDateString()}
                </td>

                <td className="px-6 py-4">
                  <ApplicationStatusBadge status={application.status} />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}