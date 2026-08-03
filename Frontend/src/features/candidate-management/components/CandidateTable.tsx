import type { User } from "../../../shared/types/user";
import CandidateTableRow from "./CandidateTableRow";

interface CandidateTableProps {
  candidates: User[];
}

export default function CandidateTable({ candidates }: CandidateTableProps) {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="overflow-x-auto">
        <table className="min-w-full border-collapse text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D]">
                Candidate
              </th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">
                Phone
              </th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">
                Status
              </th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">
                Joined
              </th>
              <th className="px-6 py-4 text-center font-semibold text-[#75837D]">
                Action
              </th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7]">
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