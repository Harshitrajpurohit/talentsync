import { useMemo, useState } from "react";

import { useApplications } from "../hooks/applications/useApplications";

import ApplicationFilters from "../components/applications/ApplicationFilters";

import ApplicationsTable from "../components/applications/ApplicationsTable";
import ApplicationCard from "../components/applications/ApplicationCard";
import ApplicationSkeleton from "../components/applications/ApplicationSkeleton";
import EmptyApplications from "../components/applications/EmptyApplications";

import type { ApplicationStatus } from "../../../shared/types/recruitment";
import SearchBar from "../../../shared/components/SearchBar";

export default function CandidateApplicationsPage() {
  const { applications, loading } = useApplications();

  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<ApplicationStatus | "All">("All");

  const filteredApplications = useMemo(() => {
    return applications.filter((application) => {
      const matchesSearch = application.jobTitle
        .toLowerCase()
        .includes(search.toLowerCase());

      const matchesStatus =
        status === "All" || application.status === status;

      return matchesSearch && matchesStatus;
    });
  }, [applications, search, status]);

  if (loading) {
    return (
      <div className="space-y-4">
        <ApplicationSkeleton />
        <ApplicationSkeleton />
        <ApplicationSkeleton />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-3xl font-bold text-gray-900 dark:text-white">
          My Applications
        </h1>

        <p className="mt-1 text-sm text-gray-500 dark:text-gray-400">
          Track the status of your job applications.
        </p>
      </div>

      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <SearchBar
          value={search}
          onChange={setSearch}
        />

        <ApplicationFilters
          value={status}
          onChange={setStatus}
        />
      </div>

      {filteredApplications.length === 0 ? (
        <EmptyApplications />
      ) : (
        <>
          <div className="hidden lg:block">
            <ApplicationsTable
              applications={filteredApplications}
            />
          </div>

          <div className="grid gap-4 lg:hidden">
            {filteredApplications.map((application) => (
              <ApplicationCard
                key={application.id}
                application={application}
              />
            ))}
          </div>
        </>
      )}
    </div>
  );
}