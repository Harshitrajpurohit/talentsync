import { useState } from "react";

import ScreeningDialog from "../../hr/components/applications/dialogs/ScreeningDialog";
import ScheduleInterviewDialog from "../../hr/components/applications/dialogs/ScheduleInterviewDialog";
import SelectionDecisionDialog from "../../hr/components/applications/dialogs/SelectionDecisionDialog";

import Pagination from "../../../shared/components/Pagination";

import { useApplications } from "../hooks/useApplications";
import { useDebounce } from "../../../shared/hooks/useDebounce";

import type { ApplicationWithDetails } from "../types/application";
import type { ApplicationStatus } from "../../../shared/types/recruitment";

import {
  ApplicationFilters,
  ApplicationSkeleton,
  ApplicationTable,
  ApplicationEmpty,
} from "../components";

export default function ApplicationsPage() {
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);

  const [status, setStatus] = useState("");
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);

  const [screeningApplication, setScreeningApplication] =
    useState<ApplicationWithDetails | null>(null);

  const [interviewApplication, setInterviewApplication] =
    useState<ApplicationWithDetails | null>(null);

  const [selectionApplication, setSelectionApplication] =
    useState<ApplicationWithDetails | null>(null);

  const { applications, loading, refetch } = useApplications(
    pageNumber,
    pageSize,
    debouncedSearch,
    status ? (status as ApplicationStatus) : undefined
  );

  function handleSuccess() {
    refetch();

    setScreeningApplication(null);
    setInterviewApplication(null);
    setSelectionApplication(null);
  }

  return (
    <>
      <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
        {/* Header */}
        <div className="flex flex-col gap-1">
          <h1 className="text-2xl font-bold text-[#212529]">
            Applications
          </h1>

          <p className="text-sm text-[#75837D]">
            Manage and track candidate applications across all job
            postings.
          </p>
        </div>

        {/* Filters */}
        <ApplicationFilters
          search={search}
          status={status}
          onSearchChange={(value) => {
            setSearch(value);
            setPageNumber(1);
          }}
          onStatusChange={(value) => {
            setStatus(value);
            setPageNumber(1);
          }}
        />

        {/* Content */}
        {loading ? (
          <ApplicationSkeleton />
        ) : applications && applications.data.length > 0 ? (
          <div className="space-y-4">
            <ApplicationTable
              applications={applications}
              onScreening={setScreeningApplication}
              onInterview={setInterviewApplication}
              onSelection={setSelectionApplication}
            />

            <Pagination
              pageNumber={applications.pageNumber}
              pageSize={applications.pageSize}
              totalRecords={applications.totalRecords}
              onPageChange={setPageNumber}
            />
          </div>
        ) : (
          <ApplicationEmpty
            message={
              search || status
                ? "No applications found matching your filters."
                : "No applications have been submitted yet."
            }
          />
        )}
      </div>

      {/* Dialogs */}

      <ScreeningDialog
        open={!!screeningApplication}
        applicationId={screeningApplication?.id ?? ""}
        onClose={() => setScreeningApplication(null)}
        onSuccess={handleSuccess}
      />

      <ScheduleInterviewDialog
        open={!!interviewApplication}
        applicationId={interviewApplication?.id ?? ""}
        onClose={() => setInterviewApplication(null)}
        onSuccess={handleSuccess}
      />

      <SelectionDecisionDialog
        open={!!selectionApplication}
        applicationId={selectionApplication?.id ?? ""}
        onClose={() => setSelectionApplication(null)}
        onSuccess={handleSuccess}
      />
    </>
  );
}