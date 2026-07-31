import type { ApplicationWithDetails } from "../../../../application/types/application";

interface CandidateApplicationsTableProps {
  applications: ApplicationWithDetails[];
  loading?: boolean;
}

export default function CandidateApplicationsTable({
  applications,
  loading = false,
}: CandidateApplicationsTableProps) {


  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">

        <div className="mb-5 h-6 w-40 animate-pulse rounded bg-gray-200 dark:bg-gray-700" />

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
          Applications
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
                Applied
              </th>

              <th className="px-6 py-4 text-left text-xs font-semibold uppercase">
                Status
              </th>
            </tr>
          </thead>


          <tbody>

            {applications.map((application) => (
              <tr
                key={application.id}
                className="border-t border-[#E5EAE7] dark:border-[#315343]"
              >

                <td className="px-6 py-4 font-medium dark:text-white">
                  {application.jobTitle}
                </td>


                <td className="px-6 py-4">
                  {new Date(
                    application.submittedDate
                  ).toLocaleDateString()}
                </td>


                <td className="px-6 py-4">
                  {application.status}
                </td>

              </tr>
            ))}

          </tbody>

        </table>


        {applications.length === 0 && (
          <div className="py-10 text-center text-[#75837D]">
            No applications found.
          </div>
        )}

      </div>

    </div>
  );
}