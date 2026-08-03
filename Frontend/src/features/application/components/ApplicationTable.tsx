import ApplicationTableRow from "./ApplicationTableRow";
import type { PaginationResponse } from "../../../shared/types/pagination";
import type { ApplicationWithDetails } from "../../application/types/application";

interface ApplicationTableProps {
  applications: PaginationResponse<ApplicationWithDetails>;
  onScreening: (application: ApplicationWithDetails) => void;
  onInterview: (application: ApplicationWithDetails) => void;
  onSelection: (application: ApplicationWithDetails) => void;
}

export default function ApplicationTable({
  applications,
  onScreening,
  onInterview,
  onSelection,
}: ApplicationTableProps) {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="overflow-x-auto">
        <table className="min-w-full text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Candidate</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Job</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Applied</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Status</th>
              <th className="px-6 py-4 text-right font-semibold text-[#75837D]">Actions</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7]">
            {applications.data.map((application) => (
              <ApplicationTableRow
                key={application.id}
                application={application}
                onScreening={onScreening}
                onInterview={onInterview}
                onSelection={onSelection}
              />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}