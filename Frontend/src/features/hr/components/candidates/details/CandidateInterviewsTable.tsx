import type { InterviewDetailed } from "../../../../interviews/types/interview";

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
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">

        <div className="mb-5 h-6 w-32 animate-pulse rounded bg-gray-200 dark:bg-gray-700" />

        <div className="space-y-4">

          {[1, 2, 3].map((item) => (
            <div
              key={item}
              className="h-14 animate-pulse rounded-xl bg-gray-100 dark:bg-gray-700"
            />
          ))}

        </div>

      </div>
    );
  }


  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">

      <div className="border-b border-[#E5EAE7] px-6 py-4 dark:border-[#315343]">
        <h2 className="text-lg font-semibold dark:text-white">
          Interviews
        </h2>
      </div>


      <div className="overflow-x-auto">

        <table className="min-w-full">

          <thead className="bg-[#F8F9FA] dark:bg-[#315343]">
            <tr>
              <th className="px-6 py-4 text-left text-xs font-semibold uppercase">
                Job
              </th>

              <th className="px-6 py-4 text-left text-xs font-semibold uppercase">
                Interviewer
              </th>

              <th className="px-6 py-4 text-left text-xs font-semibold uppercase">
                Scheduled
              </th>

              <th className="px-6 py-4 text-left text-xs font-semibold uppercase">
                Status
              </th>
            </tr>
          </thead>


          <tbody>

            {interviews.map((interview) => (
              <tr
                key={interview.id}
                className="border-t border-[#E5EAE7] dark:border-[#315343]"
              >

                <td className="px-6 py-4 font-medium dark:text-white">
                  {interview.jobTitle}
                </td>

                <td className="px-6 py-4">
                  {interview.interviewerName}
                </td>

                <td className="px-6 py-4">
                  {new Date(
                    interview.scheduledAt
                  ).toLocaleDateString()}
                </td>

                <td className="px-6 py-4">
                  {interview.status}
                </td>

              </tr>
            ))}

          </tbody>

        </table>


        {interviews.length === 0 && (
          <div className="py-10 text-center text-[#75837D]">
            No interviews scheduled.
          </div>
        )}

      </div>

    </div>
  );
}