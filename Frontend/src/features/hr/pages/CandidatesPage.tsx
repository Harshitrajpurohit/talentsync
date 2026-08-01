import { useMemo, useState } from "react";

import CandidateFilters from "../components/candidates/CandidateFilters";
import CandidateTable from "../components/candidates/CandidateTable";
import CandidateEmpty from "../components/candidates/CandidateEmpty";
import CandidateSkeleton from "../components/candidates/CandidateSkeleton";
import { useCandidates } from "../hooks/candidate/useCandidates";
import type { User, UserStatus } from "../../../shared/types/user";
import SearchBar from "../../../shared/components/SearchBar";
import Pagination from "../../../shared/components/Pagination";

export default function CandidatesPage() {
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<UserStatus | "All">("All");

  const { candidates, loading, error } = useCandidates(page, 10);

  const filteredCandidates = useMemo(() => {
    if (!candidates) return [];

    return candidates.data.filter((candidate: User) => {
      const matchesSearch =
        candidate.name.toLowerCase().includes(search.toLowerCase()) ||
        candidate.email.toLowerCase().includes(search.toLowerCase());

      const matchesStatus = status === "All" || candidate.status === status;

      return matchesSearch && matchesStatus;
    });
  }, [candidates, search, status]);

  if (loading) {
    return (
      <div className="space-y-6">
        <h1 className="text-2xl font-bold text-[#212529]">Candidates</h1>
        <CandidateSkeleton />
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-sm font-medium text-red-600">
        {error}
      </div>
    );
  }

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      
      {/* Page Header */}
      <div className="flex flex-col gap-1">
        <h1 className="text-2xl font-bold text-[#212529]">
          Candidates
        </h1>
        <p className="text-sm text-[#75837D]">
          Browse profiles and manage candidates registered on the platform.
        </p>
      </div>

      {/* Controls / Filters */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between rounded-2xl border border-[#E5EAE7] bg-white p-4 shadow-sm">
        <div className="w-full md:max-w-md">
          <SearchBar value={search} onChange={setSearch} />
        </div>
        
        <CandidateFilters status={status} onStatusChange={setStatus} />
      </div>

      {/* Content Area */}
      {filteredCandidates.length === 0 ? (
        <CandidateEmpty />
      ) : (
        <div className="space-y-4">
          <CandidateTable candidates={filteredCandidates} />
          
          <Pagination
            pageNumber={candidates?.pageNumber ?? 1}
            pageSize={candidates?.pageSize ?? 10}
            totalRecords={candidates?.totalRecords ?? 0}
            onPageChange={setPage}
          />
        </div>
      )}
      
    </div>
  );
}