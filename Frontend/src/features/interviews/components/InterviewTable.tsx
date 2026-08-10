import type { InterviewDetailed } from "../types/interview";
import InterviewTableRow from "./InterviewTableRow";

interface InterviewTableProps {
  interviews: InterviewDetailed[];
  onRecordOutcome: (interview: InterviewDetailed) => void;
}

export default function InterviewTable({
  interviews,
  onRecordOutcome,
}: InterviewTableProps) {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="overflow-x-auto">
        <table className="min-w-full text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Candidate</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Job</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Scheduled</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Location</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Status</th>
              <th className="px-6 py-4 text-right font-semibold text-[#75837D]">Action</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7]">
            {interviews.map((interview) => (
              <InterviewTableRow
                key={interview.id}
                interview={interview}
                onRecordOutcome={onRecordOutcome}
              />
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}