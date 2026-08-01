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

  const { candidate, loading, error } = useCandidate(candidateId);
  const { resume, loading: resumeLoading } = useCandidateResume(candidateId);
  const { applications, loading: applicationsLoading } = useCandidateApplications(candidateId, 1, 10);
  const { interviews, loading: interviewsLoading } = useCandidateInterviews(candidateId, 1, 10);

  if (!id) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-10 text-center shadow-sm">
        <h2 className="text-xl font-bold text-[#212529]">
          Invalid candidate ID
        </h2>
        <p className="mt-2 text-sm font-medium text-[#75837D]">
          Candidate identifier is missing.
        </p>
      </div>
    );
  }

  if (loading) return <CandidateProfileSkeleton />;

  if (error) {
    return (
      <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-sm font-medium text-red-600">
        {error}
      </div>
    );
  }

  if (!candidate) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-10 text-center shadow-sm">
        <h2 className="text-xl font-bold text-[#212529]">
          Candidate not found
        </h2>
        <p className="mt-2 text-sm font-medium text-[#75837D]">
          The requested candidate does not exist or has been removed.
        </p>
      </div>
    );
  }

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      <CandidateProfileHeader candidate={candidate} />

      <div className="grid gap-6 lg:grid-cols-3 items-start">
        {/* Left Section (Sticky on desktop) */}
        <div className="space-y-6 lg:col-span-1 lg:sticky lg:top-8">
          <CandidateInformationCard candidate={candidate} />
          <CandidateSocialLinksCard candidate={candidate} />
        </div>

        {/* Right Section */}
        <div className="space-y-6 lg:col-span-2">
          <CandidateResumeCard resume={resume} loading={resumeLoading} />
          <CandidateApplicationsTable applications={applications?.data ?? []} loading={applicationsLoading} />
          <CandidateInterviewsTable interviews={interviews?.data ?? []} loading={interviewsLoading} />
        </div>
      </div>
    </div>
  );
}