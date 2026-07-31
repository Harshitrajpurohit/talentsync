import { useParams } from "react-router-dom";

import CandidateProfileHeader from "../components/candidates/details/CandidateProfileHeader";
import CandidateInformationCard from "../components/candidates/details/CandidateInformationCard";
import CandidateSocialLinksCard from "../components/candidates/details/CandidateSocialLinksCard";
import CandidateResumeCard from "../components/candidates/details/CandidateResumeCard";
import CandidateApplicationsTable from "../components/candidates/details/CandidateApplicationsTable";
import CandidateInterviewsTable from "../components/candidates/details/CandidateInterviewsTable";
import CandidateProfileSkeleton from "../components/candidates/details/CandidateProfileSkeleton";

import { useCandidate } from "../hooks/candidate/useCandidate";
import { useCandidateResume } from "../hooks/candidate/useCandidateResume";
import { useCandidateApplications } from "../hooks/candidate/useCandidateApplications";
import { useCandidateInterviews } from "../hooks/candidate/useCandidateInterviews";

export default function CandidateDetailsPage() {
  const { id } = useParams<{ id: string }>();

  const candidateId = id ?? "";

  const {
    candidate,
    loading,
    error,
  } = useCandidate(candidateId);


  const {
    resume,
    loading: resumeLoading,
  } = useCandidateResume(candidateId);


  const {
    applications,
    loading: applicationsLoading,
  } = useCandidateApplications(
    candidateId,
    1,
    10
  );


  const {
    interviews,
    loading: interviewsLoading,
  } = useCandidateInterviews(
    candidateId,
    1,
    10
  );


  if (!id) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-10 text-center shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
        <h2 className="text-xl font-semibold text-gray-900 dark:text-white">
          Invalid candidate id
        </h2>

        <p className="mt-2 text-[#75837D] dark:text-gray-300">
          Candidate identifier is missing.
        </p>
      </div>
    );
  }


  if (loading) {
    return <CandidateProfileSkeleton />;
  }


  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-red-600 dark:border-red-900 dark:bg-red-950/40 dark:text-red-300">
        {error}
      </div>
    );
  }


  if (!candidate) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-10 text-center shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
        <h2 className="text-xl font-semibold text-gray-900 dark:text-white">
          Candidate not found
        </h2>

        <p className="mt-2 text-[#75837D] dark:text-gray-300">
          The requested candidate does not exist or has been removed.
        </p>
      </div>
    );
  }


  return (
    <div className="space-y-6">

      <CandidateProfileHeader
        candidate={candidate}
      />


      <div className="grid gap-6 lg:grid-cols-3">

        {/* Left Section */}
        <div className="space-y-6">

          <CandidateInformationCard
            candidate={candidate}
          />


          <CandidateSocialLinksCard
            candidate={candidate}
          />

        </div>


        {/* Right Section */}
        <div className="space-y-6 lg:col-span-2">


          <CandidateResumeCard
            resume={resume}
            loading={resumeLoading}
          />


          <CandidateApplicationsTable
            applications={applications?.data ?? []}
            loading={applicationsLoading}
          />


          <CandidateInterviewsTable
            interviews={interviews?.data ?? []}
            loading={interviewsLoading}
          />


        </div>

      </div>

    </div>
  );
}