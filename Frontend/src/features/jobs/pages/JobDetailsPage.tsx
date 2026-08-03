import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import JobHeader from "../components/details/JobHeader";
import JobInformation from "../components/details/JobInformation";
import JobSummaryCards from "../components/details/JobSummaryCards";
import JobApplicationTable from "../components/details/JobApplicationTable";
import JobApplicationEmpty from "../components/details/JobApplicationEmpty";
import JobApplicationSkeleton from "../components/details/JobApplicationSkeleton";
import JobDetailsSkeleton from "../components/details/JobDetailsSkeleton";
import JobDetailsError from "../components/details/JobDetailsError";
import JobDetailsActions from "../components/details/JobDetailsActions";

import EditJobDialog from "../../recruiter/dialogs/EditJobDialog";
import UpdateJobStatusDialog from "../../recruiter/dialogs/UpdateJobStatusDialog";

import { useJob } from "../hooks/useJob";
import { useJobSummary } from "../hooks/useJobSummary";
import { useJobApplications } from "../hooks/useJobApplications";

import type { JobDetails } from "../types/job";

export default function JobDetailsPage() {
  const { id = "" } = useParams();
  const navigate = useNavigate();

  const [editJob, setEditJob] = useState<JobDetails | null>(null);
  const [statusJob, setStatusJob] = useState<JobDetails | null>(null);

  const { job, loading, error, refetch } = useJob(id);
  const { summary, refetch: refetchSummary } = useJobSummary(id);
  const {
    applications,
    loading: applicationLoading,
    refetch: refetchApplications,
  } = useJobApplications(id, 1, 10);

  function handleEdit() {
    if (job) setEditJob(job);
  }

  function handleStatus() {
    if (job) setStatusJob(job);
  }

  function handleSuccess() {
    refetch();
    refetchSummary();
    refetchApplications();
    setEditJob(null);
    setStatusJob(null);
  }

  if (loading) {
    return <JobDetailsSkeleton />;
  }

  if (error || !job) {
    return (
      <JobDetailsError
        message={error ?? "Unable to load job details."}
      />
    );
  }

  return (
    <>
      <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
        
        {/* Actions Bar */}
        <JobDetailsActions
          job={job}
          onBack={() => navigate(-1)}
          onEdit={handleEdit}
          onStatus={handleStatus}
        />

        {/* Main Job Details Header */}
        <JobHeader job={job} />

        {/* Summary Stats */}
        {summary && <JobSummaryCards summary={summary} />}

        {/* Descriptions & Requirements */}
        <JobInformation job={job} />

        {/* Applications List */}
        {applicationLoading ? (
          <JobApplicationSkeleton />
        ) : applications && applications.data.length > 0 ? (
          <JobApplicationTable applications={applications.data} />
        ) : (
          <JobApplicationEmpty />
        )}
      </div>

      {/* Dialogs */}
      <EditJobDialog
        open={!!editJob}
        job={editJob}
        onClose={() => setEditJob(null)}
        onSuccess={handleSuccess}
      />

      <UpdateJobStatusDialog
        open={!!statusJob}
        job={statusJob}
        onClose={() => setStatusJob(null)}
        onSuccess={handleSuccess}
      />
    </>
  );
}