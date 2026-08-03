import { useMemo, useState } from "react";
import { Plus } from "lucide-react";
import { useNavigate } from "react-router-dom"; // 1. Import useNavigate

import SearchBar from "../../../shared/components/SearchBar";
import Pagination from "../../../shared/components/Pagination";

import JobTable from "../components/JobTable";
import JobFilters from "../components/JobFilters";
import JobEmpty from "../components/JobEmpty";
import JobSkeleton from "../components/JobSkeleton";

import CreateJobDialog from "../../recruiter/dialogs/CreateJobDialog";
import EditJobDialog from "../../recruiter/dialogs/EditJobDialog";
import UpdateJobStatusDialog from "../../recruiter/dialogs/UpdateJobStatusDialog";

import { useJobs } from "../hooks/useJobs";
import { getJobById } from "../api/jobApi";

import type { JobDetails, JobListItem } from "../types/job";
import type { JobStatus } from "../../../shared/types/jobs";
import { getAuth } from "../../../shared/api/authStorage";

export default function JobsPage() {
  const navigate = useNavigate(); // 2. Initialize navigate

  const [page, setPage] = useState(1);
  const [search, setSearch] = useState("");
  const [status, setStatus] = useState<JobStatus | "All">("All");

  const [createOpen, setCreateOpen] = useState(false);
  const [editJob, setEditJob] = useState<JobDetails | null>(null);
  const [statusJob, setStatusJob] = useState<JobDetails | null>(null);

  const { jobs, loading, error, refetch } = useJobs(page, 10);

  const role = getAuth()?.role.toLowerCase();
  
  function handleView(job: JobListItem) {
    navigate(`/${role}/jobs/${job.id}`);
  }

  async function handleEdit(job: JobListItem) {
    try {
      const details = await getJobById(job.id);
      setEditJob(details);
    } catch (error) {
      console.error(error);
    }
  }

  async function handleStatus(job: JobListItem) {
    try {
      const details = await getJobById(job.id);
      setStatusJob(details);
    } catch (error) {
      console.error(error);
    }
  }

  const filteredJobs = useMemo(() => {
    if (!jobs) return [];
    return jobs.data.filter((job) => {
      const matchesSearch =
        job.title.toLowerCase().includes(search.toLowerCase()) ||
        job.department.toLowerCase().includes(search.toLowerCase());
      const matchesStatus = status === "All" || job.status === status;
      return matchesSearch && matchesStatus;
    });
  }, [jobs, search, status]);

  if (loading) {
    return (
      <div className="space-y-6">
        <h1 className="text-2xl font-bold text-[#212529] dark:text-white">Jobs</h1>
        <JobSkeleton />
      </div>
    );
  }

  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-sm font-medium text-red-600 dark:border-red-900/50 dark:bg-red-900/20 dark:text-red-400">
        {error}
      </div>
    );
  }

  return (
    <>
      <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
        <div className="flex flex-col gap-4 md:flex-row md:items-end md:justify-between">
          <div>
            <h1 className="text-2xl font-bold text-[#212529] dark:text-white">Jobs</h1>
            <p className="mt-1 text-sm text-[#75837D] dark:text-white/70">
              Manage all job openings and postings.
            </p>
          </div>
          {role === "recruiter" && (
          <button
            type="button"
            onClick={() => setCreateOpen(true)}
            className="inline-flex items-center justify-center gap-2 rounded-full bg-[#315343] px-6 py-2.5 text-sm font-bold text-white shadow-sm transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 dark:bg-[#1E3329] dark:text-[#C3F53C] dark:hover:bg-[#C3F53C] dark:hover:text-[#1E3329]"
          >
            <Plus size={18} className="text-current" />
            Create Job
          </button>)
          }
        </div>

        <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between rounded-2xl border border-[#E5EAE7] bg-white p-4 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
          <div className="w-full md:max-w-md">
            <SearchBar value={search} onChange={setSearch} />
          </div>
          <JobFilters status={status} onStatusChange={setStatus} />
        </div>

        {filteredJobs.length === 0 ? (
          <JobEmpty />
        ) : (
          <div className="space-y-4">
            <JobTable jobs={filteredJobs} onView={handleView} onEdit={handleEdit} onStatus={handleStatus} />
            <Pagination
              pageNumber={jobs!.pageNumber}
              pageSize={jobs!.pageSize}
              totalRecords={jobs!.totalRecords}
              onPageChange={setPage}
            />
          </div>
        )}
      </div>

      <CreateJobDialog open={createOpen} onClose={() => setCreateOpen(false)} onSuccess={refetch} />
      <EditJobDialog open={!!editJob} job={editJob} onClose={() => setEditJob(null)} onSuccess={refetch} />
      <UpdateJobStatusDialog open={!!statusJob} job={statusJob} onClose={() => setStatusJob(null)} onSuccess={refetch} />
    </>
  );
}