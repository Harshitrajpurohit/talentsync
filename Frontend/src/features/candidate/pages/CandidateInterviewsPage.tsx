import { useMemo, useState } from "react";
import { useDebounce } from "../../../shared/hooks/useDebounce";
import { useInterviews } from "../hooks/interviews/useInterviews";
import type { InterviewPaginationRequest } from "../../../shared/types/pagination";
import type { InterviewStatus } from "../../../shared/types/recruitment";

import {
  InterviewCardList,
  InterviewEmpty,
  InterviewFilters,
  InterviewHeader,
  InterviewPagination,
  InterviewSkeleton,
} from "../components/interviews";

export default function CandidateInterviewsPage() {
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(6);

  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<InterviewStatus | "">("");

  const [fromDate, setFromDate] = useState("");
  const [toDate, setToDate] = useState("");

  const debouncedSearch = useDebounce(search, 500);

  const paginationRequest = useMemo<InterviewPaginationRequest>(
    () => ({
      pageNumber,
      pageSize,
      search: debouncedSearch || undefined,
      status: status || undefined,
      fromDate: fromDate || undefined,
      toDate: toDate || undefined,
    }),
    [pageNumber, pageSize, debouncedSearch, status, fromDate, toDate]
  );

  const { interviews, loading, error } = useInterviews(paginationRequest);

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      <InterviewHeader totalInterviews={interviews?.totalRecords ?? 0} />

      <InterviewFilters
        search={search}
        status={status}
        fromDate={fromDate}
        toDate={toDate}
        onSearchChange={(value) => {
          setSearch(value);
          setPageNumber(1);
        }}
        onStatusChange={(value) => {
          setStatus(value);
          setPageNumber(1);
        }}
        onFromDateChange={(value) => {
          setFromDate(value);
          setPageNumber(1);
        }}
        onToDateChange={(value) => {
          setToDate(value);
          setPageNumber(1);
        }}
      />

      {loading ? (
        <InterviewSkeleton />
      ) : error ? (
        <div className="rounded-xl border border-red-200 bg-red-50 p-5 text-sm font-medium text-red-600">
          {error}
        </div>
      ) : interviews?.data.length ? (
        <div className="space-y-6">
          <InterviewCardList interviews={interviews.data} />

          <InterviewPagination
            currentPage={interviews.pageNumber}
            totalPages={Math.ceil(interviews.totalRecords / interviews.pageSize)}
            onPageChange={setPageNumber}
          />
        </div>
      ) : (
        <InterviewEmpty />
      )}
    </div>
  );
}