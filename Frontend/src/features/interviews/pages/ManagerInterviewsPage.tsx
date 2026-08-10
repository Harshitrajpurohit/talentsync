import { useState } from "react";
import { CalendarDays, RefreshCw } from "lucide-react";

import {
  InterviewTable,
  InterviewFilters,
  InterviewEmpty,
  InterviewSkeleton,
  InterviewOutcomeDialog,
} from "../components";

import { useAssignedInterviews } from "../hooks/useAssignedInterviews";
import type { InterviewDetailed } from "../types/interview";
import type { InterviewPaginationRequest } from "../../../shared/types/pagination";
import type { InterviewStatus } from "../../../shared/types/recruitment";

import Pagination from "../../../shared/components/Pagination";
import { useDebounce } from "../../../shared/hooks/useDebounce";

export default function ManagerInterviewsPage() {
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);
  const [search, setSearch] = useState("");
  const debouncedSearch = useDebounce(search, 500);

  const [status, setStatus] = useState<InterviewStatus | undefined>(undefined);
  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");

  const [selectedInterview, setSelectedInterview] = useState<InterviewDetailed | null>(null);
  const [outcomeDialogOpen, setOutcomeDialogOpen] = useState(false);

  const request: InterviewPaginationRequest = {
    pageNumber,
    pageSize,
    search: debouncedSearch.trim() || undefined,
    status,
    fromDate: fromDate || undefined,
    toDate: toDate || undefined,
  };

  const { interviews, pagination, loading, error, refetch } = useAssignedInterviews(request);

  function handleSearchChange(value: string) {
    setSearch(value);
    setPageNumber(1);
  }

  function handleStatusChange(value: InterviewStatus | undefined) {
    setStatus(value);
    setPageNumber(1);
  }

  function handleFromDateChange(value: string) {
    setFromDate(value);
    setPageNumber(1);
  }

  function handleToDateChange(value: string) {
    setToDate(value);
    setPageNumber(1);
  }

  function handleClearFilters() {
    setSearch("");
    setStatus(undefined);
    setFromDate("");
    setToDate("");
    setPageNumber(1);
  }

  function handleRecordOutcome(interview: InterviewDetailed) {
    setSelectedInterview(interview);
    setOutcomeDialogOpen(true);
  }

  function handleCloseOutcomeDialog() {
    if (loading) return;
    setOutcomeDialogOpen(false);
    setSelectedInterview(null);
  }

  function handleOutcomeSuccess() {
    void refetch();
  }

  const hasFilters = Boolean(search.trim()) || Boolean(status) || Boolean(fromDate) || Boolean(toDate);

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      {/* Header */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-end sm:justify-between">
        <div>
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
              <CalendarDays size={20} />
            </div>
            <h1 className="text-2xl font-bold text-[#212529]">
              Assigned Interviews
            </h1>
          </div>
          <p className="mt-2 text-sm font-medium text-[#75837D]">
            Manage your assigned interviews and record their outcomes.
          </p>
        </div>

        <button
          type="button"
          onClick={() => void refetch()}
          disabled={loading}
          className="inline-flex items-center justify-center gap-2 rounded-full border border-[#E5EAE7] bg-white px-5 py-2.5 text-sm font-bold text-[#315343] shadow-sm transition-all hover:bg-[#EEF3F0] active:scale-95 disabled:pointer-events-none disabled:opacity-50"
        >
          <RefreshCw size={16} className={loading ? "animate-spin" : ""} />
          Refresh
        </button>
      </div>

      {/* Filters */}
      <InterviewFilters
        search={search}
        status={status}
        fromDate={fromDate}
        toDate={toDate}
        onSearchChange={handleSearchChange}
        onStatusChange={handleStatusChange}
        onFromDateChange={handleFromDateChange}
        onToDateChange={handleToDateChange}
        onClear={handleClearFilters}
      />

      {/* Error State */}
      {error && !loading && (
        <div className="flex items-center justify-between rounded-xl border border-red-200 bg-red-50 px-5 py-4">
          <p className="text-sm font-bold text-red-700">{error}</p>
          <button
            type="button"
            onClick={() => void refetch()}
            className="text-sm font-bold text-red-700 underline transition hover:text-red-800"
          >
            Retry
          </button>
        </div>
      )}

      {/* Content */}
      {loading ? (
        <InterviewSkeleton />
      ) : interviews.length === 0 ? (
        <InterviewEmpty filtered={hasFilters} />
      ) : (
        <div className="space-y-4">
          <InterviewTable
            interviews={interviews}
            onRecordOutcome={handleRecordOutcome}
          />
          <Pagination
            pageNumber={pagination.pageNumber}
            pageSize={pagination.pageSize}
            totalRecords={pagination.totalRecords}
            onPageChange={setPageNumber}
          />
        </div>
      )}

      {/* Outcome Dialog */}
      <InterviewOutcomeDialog
        open={outcomeDialogOpen}
        interview={selectedInterview}
        onClose={handleCloseOutcomeDialog}
        onSuccess={handleOutcomeSuccess}
      />
    </div>
  );
}