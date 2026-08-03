import type { InterviewDetailed } from "../../../interviews/types/interview";

interface CandidateInterviewsTableProps {
  interviews: InterviewDetailed[];
  loading?: boolean;
}

export default function CandidateInterviewsTable({
  interviews,
  loading = false,
}: CandidateInterviewsTableProps) {
  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
        <div className="mb-5 h-6 w-32 animate-pulse rounded bg-[#EEF3F0]" />
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
        <h2 className="text-lg font-bold text-[#212529]">Interviews</h2>
      </div>

      <div className="overflow-x-auto">
        <table className="min-w-full text-left text-sm">
          <thead className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
            <tr>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Job</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Interviewer</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Scheduled</th>
              <th className="px-6 py-4 font-semibold text-[#75837D]">Status</th>
            </tr>
          </thead>
          <tbody className="divide-y divide-[#E5EAE7]">
            {interviews.map((interview) => (
              <tr key={interview.id} className="transition-colors hover:bg-[#EEF3F0]/50">
                <td className="px-6 py-4 font-bold text-[#212529]">
                  {interview.jobTitle}
                </td>
                <td className="px-6 py-4 font-medium text-[#75837D]">
                  {interview.interviewerName}
                </td>
                <td className="px-6 py-4 font-medium text-[#75837D]">
                  {new Date(interview.scheduledAt).toLocaleDateString()}
                </td>
                <td className="px-6 py-4">
                  <span className="inline-flex items-center rounded-md bg-[#EEF3F0] px-2.5 py-1 text-xs font-bold text-[#315343]">
                    {interview.status}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>

        {interviews.length === 0 && (
          <div className="py-10 text-center text-sm font-medium text-[#75837D]">
            No interviews scheduled.
          </div>
        )}
      </div>
    </div>
  );
}