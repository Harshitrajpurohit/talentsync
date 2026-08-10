import { Link } from "react-router-dom";
import { ArrowRight } from "lucide-react";

import ApplicationStatusBadge from "../../../application/components/ApplicationStatusBadge";

import { getAuth } from "../../../../shared/api/authStorage";

import type { ApplicationWithDetails } from "../../../application/types/application";

interface RecentApplicationsCardProps {
  applications: ApplicationWithDetails[];
}

export default function RecentApplicationsCard({
  applications,
}: RecentApplicationsCardProps) {
  const role = getAuth()?.role?.toLowerCase();

  return (
    <div className="rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm">
      <div className="flex items-center justify-between border-b border-[#E5EAE7] px-6 py-5">
        <div>
          <h2 className="text-lg font-bold text-[#212529]">
            Recent Applications
          </h2>

          <p className="text-sm text-[#75837D]">
            Latest candidate applications.
          </p>
        </div>
      </div>

      <div className="divide-y divide-[#E5EAE7]">
        {applications.length === 0 ? (
          <div className="py-10 text-center text-sm text-[#75837D]">
            No recent applications.
          </div>
        ) : (
          applications.map((application) => (
            <div
              key={application.id}
              className="flex items-center justify-between px-6 py-4"
            >
              <div>
                <Link
                  to={`/${role}/applications/${application.id}`}
                  className="font-bold text-[#212529] hover:text-[#315343]"
                >
                  {application.candidateName}
                </Link>

                <p className="mt-1 text-sm text-[#75837D]">
                  {application.jobTitle}
                </p>

                <p className="mt-1 text-xs text-[#75837D]">
                  {new Date(application.submittedDate).toLocaleDateString()}
                </p>
              </div>

              <div className="flex items-center gap-3">
                <ApplicationStatusBadge
                  status={application.status}
                />

                <Link
                  to={`/${role}/applications/${application.id}`}
                  className="rounded-full border border-[#E5EAE7] p-2 transition hover:bg-[#EEF3F0]"
                >
                  <ArrowRight size={16} />
                </Link>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  );
}