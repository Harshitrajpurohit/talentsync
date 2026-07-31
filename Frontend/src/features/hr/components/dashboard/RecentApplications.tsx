import type { ApplicationWithDetails } from "../../../application/types/application";
import { Link } from "react-router-dom";

type Props = {
  applications: ApplicationWithDetails[];
};

export default function RecentApplications({ applications }: Props) {
  return (
    <div className="flex flex-col overflow-hidden rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="flex items-center justify-between border-b border-[#E5EAE7] p-5 dark:border-[#315343]">
        <h2 className="text-lg font-bold text-[#212529] dark:text-white">
          Recent Applications
        </h2>
        <Link
          to="/hr/applications"
          className="text-sm font-semibold text-[#75837D] transition-colors hover:text-[#315343] dark:hover:text-[#C3F53C]"
        >
          View All
        </Link>
      </div>

      <div className="overflow-x-auto">
        {applications.length === 0 ? (
          <div className="m-6 flex h-32 items-center justify-center rounded-[16px] bg-[#F8FAF9] dark:bg-[#1e3329]">
            <p className="text-sm font-medium text-[#75837D] dark:text-white/60">
              No recent applications found.
            </p>
          </div>
        ) : (
          <table className="w-full">
            <thead className="bg-[#F8FAF9] dark:bg-[#1e3329]">
              <tr>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Candidate</th>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Job</th>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Status</th>
                <th className="p-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D] dark:text-white/70">Applied</th>
              </tr>
            </thead>
            <tbody className="divide-y divide-[#E5EAE7] dark:divide-[#315343]">
              {applications.map((application) => (
                <tr key={application.id} className="group transition-colors hover:bg-[#F8FAF9]/50 dark:hover:bg-[#1e3329]/50">
                  <td className="p-4 text-sm font-bold text-[#212529] transition-colors group-hover:text-[#315343] dark:text-white dark:group-hover:text-[#C3F53C] hover:underline">
                    <Link to={`/hr/candidates/${application.candidateId}`}>
                    {application.candidateName}
                    </Link>
                  </td>
                  <td className="p-4 text-sm font-medium text-[#75837D] dark:text-white/80">{application.jobTitle}</td>
                  <td className="p-4 text-sm font-medium text-[#75837D] dark:text-white/80">
                    <span className="rounded-full bg-[#E5EAE7]/50 px-2.5 py-1 text-[11px] font-bold uppercase tracking-wide dark:bg-[#315343]/50 dark:text-white/90">
                      {application.status}
                    </span>
                  </td>
                  <td className="p-4 text-sm font-medium text-[#75837D] dark:text-white/80">
                    {new Date(application.submittedDate).toLocaleDateString("en-US", { month: "short", day: "numeric", year: "numeric" })}
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        )}
      </div>
    </div>
  );
}