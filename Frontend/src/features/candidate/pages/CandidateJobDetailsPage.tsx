import { useCallback } from "react";
import { useNavigate, useParams } from "react-router-dom";

import { useJob } from "../hooks/jobs/useJob";
import { useCreateApplication } from "../hooks/applications/useCreateApplication";

import JobDetailsCard from "../components/jobs/JobDetailsCard";
import JobSkeleton from "../components/jobs/JobSkeleton";

export default function CandidateJobDetailsPage() {
  const navigate = useNavigate();
  const { id } = useParams();

  const {
    job,
    loading,
    refresh,
  } = useJob(id ?? "");

  const {
    apply,
    loading: applying,
  } = useCreateApplication();

  const handleApply = useCallback(async () => {
    if (!job) return;

    await apply({
      jobId: job.id,
    });

    await refresh();
  }, [job, apply, refresh]);

  if (loading) {
    return <JobSkeleton />;
  }

  if (!job) {
    return (
      <div className="rounded-[20px] border border-[#E5EAE7] bg-white p-10 text-center shadow-sm dark:border-gray-700 dark:bg-gray-900">
        <h2 className="text-xl font-semibold text-[#212529] dark:text-white">
          Job not found
        </h2>

        <p className="mt-2 text-[#75837D]">
          The requested job could not be found.
        </p>

        <button
          onClick={() => navigate("/candidate/jobs")}
          className="mt-6 rounded-[20px] bg-[#315343] px-5 py-2 font-semibold text-white shadow-sm transition-all duration-300 hover:bg-[#C3F53C] hover:text-[#315343] hover:shadow-md"
        >
          Back to Jobs
        </button>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <button
        onClick={() => navigate(-1)}
        className="group flex items-center text-sm font-semibold text-[#75837D] transition-colors duration-300 hover:text-[#315343]"
      >
        <span className="mr-2 transition-transform duration-300 group-hover:-translate-x-1 group-hover:text-[#C3F53C]">
          ←
        </span>
        Back to Jobs
      </button>

      <JobDetailsCard
        job={job}
        applying={applying}
        onApply={handleApply}
      />
    </div>
  );
}