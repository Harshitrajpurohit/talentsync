import JobApplicationRow from "./JobApplicationRow";
import type { ApplicationWithDetails } from "../../../application/types/application";

interface JobApplicationTableProps {
  applications: ApplicationWithDetails[];
}

export default function JobApplicationTable({
  applications,
}: JobApplicationTableProps) {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="border-b border-[#E5EAE7] px-6 py-5 dark:border-[#315343]">
        <h2 className="text-lg font-bold text-[#212529] dark:text-white">
          Applications
        </h2>
      </div>

      <div className="overflow-x-auto">
        <table className="min-w-full text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9] dark:border-[#315343] dark:bg-[#1E3329]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Candidate</th>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Status</th>
              <th className="px-6 py-4 font-semibold text-[#75837D] dark:text-white/80">Applied On</th>
              <th className="px-6 py-4 text-right font-semibold text-[#75837D] dark:text-white/80">Action</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7] dark:divide-[#315343]">
            {applications.map((application) => (
              <JobApplicationRow
                key={application.id}
                application={application}
              />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}