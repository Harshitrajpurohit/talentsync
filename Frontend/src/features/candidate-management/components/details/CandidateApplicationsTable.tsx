import { Link } from "react-router-dom";
import { getAuth } from "../../../../shared/api/authStorage";
import type { ApplicationWithDetails } from "../../../application/types/application";

interface CandidateApplicationsTableProps {
  applications: ApplicationWithDetails[];
  loading?: boolean;
}

export default function CandidateApplicationsTable({
  applications,
  loading = false,
}: CandidateApplicationsTableProps) {
  const role = getAuth()?.role?.toLowerCase();
  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
        <div className="mb-5 h-6 w-40 animate-pulse rounded bg-[#EEF3F0]" />
        <div className="space-y-3">
          {[1, 2, 3].map((item) => (
            <div key={item} className="h-12 animate-pulse rounded-xl bg-[#EEF3F0]" />
          ))}
        </div>
      </div>
    );
  }

  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="border-b border-[#E5EAE7] px-6 py-5">
        <h2 className="text-lg font-bold text-[#212529]">Applications</h2>
      </div>

      <div className="overflow-x-auto">
        <table className="min-w-full text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Job</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Applied</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Status</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7]">
            {applications.map((application) => (
              <tr key={application.id} className="transition-colors hover:bg-[#EEF3F0]/50">
                <td className="px-6 py-4 font-bold text-[#212529]">
                  <Link to={`/${role}/jobs/${application.jobId}`} className="hover:underline">
                    {application.jobTitle}
                  </Link>
                </td>
                <td className="px-6 py-4 font-medium text-[#75837D]">
                  {new Date(application.submittedDate).toLocaleDateString()}
                </td>
                <td className="px-6 py-4">
                  <span className="inline-flex items-center rounded-md bg-[#EEF3F0] px-2.5 py-1 text-xs font-bold text-[#315343]">
                    {application.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {applications.length === 0 && (
          <div className="py-10 text-center text-sm font-medium text-[#75837D]">
            No applications found.
          </div>
        )}
      </div>
    </div>
  );
}