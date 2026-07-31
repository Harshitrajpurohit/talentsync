import type { User } from "../../../../shared/types/user";
import CandidateTableRow from "./CandidateTableRow";

interface CandidateTableProps {
  candidates: User[];
}

export default function CandidateTable({
  candidates,
}: CandidateTableProps) {
  return (
    <div className="overflow-hidden rounded-[20px] border border-gray-200 bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="overflow-x-auto">
        <table className="w-full whitespace-nowrap">
          {/* Matches the clean, white header from the image */}
          <thead className="bg-white dark:bg-[#253f33]">
            <tr>
              <th className="px-6 py-5 text-left text-[14px] font-bold text-gray-900 dark:text-white">
                Candidate
              </th>
              <th className="px-6 py-5 text-left text-[14px] font-bold text-gray-900 dark:text-white">
                Phone
              </th>
              <th className="px-6 py-5 text-left text-[14px] font-bold text-gray-900 dark:text-white">
                Status
              </th>
              <th className="px-6 py-5 text-left text-[14px] font-bold text-gray-900 dark:text-white">
                Joined
              </th>
              <th className="px-6 py-5 text-center text-[14px] font-bold text-gray-900 dark:text-white">
                Action
              </th>
            </tr>
          </thead>
          <tbody className="dark:divide-[#315343]">
            {candidates.map((candidate) => (
              <CandidateTableRow
                key={candidate.id}
                candidate={candidate}
              />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}