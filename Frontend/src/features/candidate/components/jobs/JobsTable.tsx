import { CheckCircle, Minus } from "lucide-react";
import type { CandidateJob } from "../../types/job";
import JobStatusBadge from "./JobStatusBadge";

interface JobsTableProps {
  jobs: CandidateJob[];
  onView: (id: string) => void;
}

export default function JobsTable({
  jobs,
  onView,
}: JobsTableProps) {
  return (
    <div className="overflow-hidden rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <div className="overflow-x-auto">
        <table className="min-w-full">
          <thead className="bg-[#E5EAE7]/20 dark:bg-gray-800">
            <tr>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Title
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Department
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Posted
              </th>
              <th className="px-6 py-4 text-left text-sm font-semibold text-[#212529] dark:text-gray-300">
                Status
              </th>
              <th className="px-6 py-4 text-center text-sm font-semibold text-[#212529] dark:text-gray-300">
                Applied
              </th>
              <th className="px-6 py-4 text-right text-sm font-semibold text-[#212529] dark:text-gray-300">
                Action
              </th>
            </tr>
          </thead>

          <tbody>
            {jobs.map((job) => (
              <tr
                key={job.id}
                className="group border-t border-[#E5EAE7] transition-colors duration-300 hover:bg-[#E5EAE7]/10 dark:border-gray-700 dark:hover:bg-gray-800/50"
              >
                <td className="px-6 py-4 font-medium text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
                  {job.title}
                </td>

                <td className="px-6 py-4 text-sm text-[#75837D]">
                  {job.department}
                </td>

                <td className="px-6 py-4 text-sm text-[#75837D]">
                  {new Date(job.postedDate).toLocaleDateString()}
                </td>

                <td className="px-6 py-4">
                  <JobStatusBadge status={job.status} />
                </td>

                {/* Updated Applied Status Icon */}
                <td className="px-6 py-4">
                  <div className="flex justify-center">
                    {job.hasApplied ? (
                      <CheckCircle 
                        size={20} 
                        className="text-[#315343] transition-colors duration-300 group-hover:text-[#C3F53C]" 
                      />
                    ) : (
                      <Minus size={20} className="text-[#E5EAE7]" />
                    )}
                  </div>
                </td>

                <td className="px-6 py-4 text-right">
                  <button
                    onClick={() => onView(job.id)}
                    className="rounded-[20px] bg-[#315343] px-4 py-2 text-sm font-semibold text-white shadow-sm transition-all duration-300 hover:bg-[#C3F53C] hover:text-[#315343] hover:shadow-md"
                  >
                    View
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}