export default function InterviewSkeleton() {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="overflow-x-auto">
        <table className="w-full min-w-[900px]">
          <thead>
            <tr className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
              {["Candidate", "Job", "Scheduled", "Location", "Status", "Action"].map((header) => (
                <th
                  key={header}
                  className="px-6 py-4 text-left text-xs font-semibold text-[#75837D]"
                >
                  {header}
                </th>
              ))}
            </tr>
          </thead>
          <tbody>
            {Array.from({ length: 6 }).map((_, index) => (
              <tr key={index} className="border-b border-[#E5EAE7] last:border-0">
                <td className="px-6 py-5">
                  <div className="space-y-2">
                    <div className="h-4 w-32 animate-pulse rounded bg-[#EEF3F0]" />
                    <div className="h-3 w-40 animate-pulse rounded bg-[#EEF3F0]" />
                  </div>
                </td>
                <td className="px-6 py-5">
                  <div className="h-4 w-36 animate-pulse rounded bg-[#EEF3F0]" />
                </td>
                <td className="px-6 py-5">
                  <div className="space-y-2">
                    <div className="h-4 w-24 animate-pulse rounded bg-[#EEF3F0]" />
                    <div className="h-3 w-16 animate-pulse rounded bg-[#EEF3F0]" />
                  </div>
                </td>
                <td className="px-6 py-5">
                  <div className="h-4 w-24 animate-pulse rounded bg-[#EEF3F0]" />
                </td>
                <td className="px-6 py-5">
                  <div className="h-6 w-20 animate-pulse rounded-md bg-[#EEF3F0]" />
                </td>
                <td className="px-6 py-5">
                  <div className="ml-auto h-8 w-28 animate-pulse rounded-full bg-[#EEF3F0]" />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}