import { useMemo, useState } from "react";
import { useNavigate } from "react-router-dom";

import { useJobs } from "../hooks/jobs/useJobs";

import JobSearch from "../components/jobs/JobSearch";
import JobFilters from "../components/jobs/JobFilters";
import JobsTable from "../components/jobs/JobsTable";
import JobCard from "../components/jobs/JobCard";
import JobSkeleton from "../components/jobs/JobSkeleton";
import EmptyJobs from "../components/jobs/EmptyJobs";
import JobPagination from "../components/jobs/JobPagination";

import type { JobStatus } from "../../../shared/types/recruitment";

export default function CandidateJobsPage() {
  const navigate = useNavigate();

  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 10;

  const { jobs, pagination, loading } = useJobs(pageNumber, pageSize);

  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<JobStatus | "All">("All");

  const filteredJobs = useMemo(() => {
    return jobs.filter((job) => {
      const matchesSearch =
        job.title.toLowerCase().includes(search.toLowerCase()) ||
        job.department.toLowerCase().includes(search.toLowerCase());

      const matchesStatus =
        status === "All" || job.status === status;

      return matchesSearch && matchesStatus;
    });
  }, [jobs, search, status]);

  if (loading) {
    return (
      <div className="space-y-4">
        <JobSkeleton />
        <JobSkeleton />
        <JobSkeleton />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div>
        <h1 className="text-2xl font-bold text-[#212529] dark:text-white">
          Available Jobs
        </h1>

        <p className="mt-1 text-sm text-[#75837D] dark:text-gray-400">
          Browse open positions and apply for opportunities that match your
          skills.
        </p>
      </div>

      {/* Search & Filter */}
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <JobSearch value={search} onChange={setSearch} />

        <JobFilters value={status} onChange={setStatus} />
      </div>

      {/* Jobs */}
      {filteredJobs.length === 0 ? (
        <EmptyJobs />
      ) : (
        <>
          {/* Desktop */}
          <div className="hidden lg:block">
            <JobsTable
              jobs={filteredJobs}
              onView={(id) => navigate(`/candidate/jobs/${id}`)}
            />
          </div>

          {/* Mobile */}
          <div className="grid gap-4 lg:hidden">
            {filteredJobs.map((job) => (
              <JobCard
                key={job.id}
                job={job}
                onView={(id) => navigate(`/candidate/jobs/${id}`)}
              />
            ))}
          </div>

          {/* Pagination */}
          {pagination && (
            <JobPagination
              pageNumber={pagination.pageNumber}
              pageSize={pagination.pageSize}
              totalRecords={pagination.totalRecords}
              onPageChange={setPageNumber}
            />
          )}
        </>
      )}
    </div>
  );
}