import { useMemo, useState } from "react";

import CandidateSearch from "../components/candidates/CandidateSearch";
import CandidateFilters from "../components/candidates/CandidateFilters";
import CandidateTable from "../components/candidates/CandidateTable";
import CandidatePagination from "../components/candidates/CandidatePagination";
import CandidateEmpty from "../components/candidates/CandidateEmpty";
import CandidateSkeleton from "../components/candidates/CandidateSkeleton";
import { useCandidates } from "../hooks/candidate/useCandidates";
import type { User, UserStatus } from "../../../shared/types/user";

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

  const totalPages = candidates
    ? Math.ceil(candidates.totalRecords / candidates.pageSize)
    : 1;

  if (loading) return <CandidateSkeleton />;
  if (error) return <div className="text-red-500">{error}</div>;

  return (
    <div className="w-full animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both space-y-6 sm:space-y-8">
      
      {/* Title & Subtitle Matching the Image */}
      <div>
        <h1 className="text-2xl font-bold text-gray-900 dark:text-white">
          Candidates
        </h1>
        <p className="mt-1 text-sm text-gray-500 dark:text-white/60">
          Browse profiles and manage candidates registered on the platform.
        </p>
      </div>

      {/* Pill-shaped controls */}
      <div className="flex flex-col gap-4 sm:flex-row sm:items-center sm:justify-between">
        <CandidateSearch value={search} onChange={setSearch} />
        <CandidateFilters status={status} onStatusChange={setStatus} />
      </div>

      {/* Table Area */}
      {filteredCandidates.length === 0 ? (
        <CandidateEmpty />
      ) : (
        <div className="space-y-6">
          <CandidateTable candidates={filteredCandidates} />
          
          <CandidatePagination
            currentPage={candidates?.pageNumber ?? 1}
            totalPages={totalPages}
            onPageChange={setPage}
          />
        </div>
      )}
      
    </div>
  );
}