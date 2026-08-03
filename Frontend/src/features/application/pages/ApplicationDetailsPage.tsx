import { useState } from "react";
import { useNavigate, useParams } from "react-router-dom";

import ApplicationActions from "../components/details/ApplicationActions";
import ApplicationHeader from "../components/details/ApplicationHeader";
import CandidateInformation from "../components/details/CandidateInformation";
import JobInformation from "../components/details/JobInformation";
import ApplicationInformation from "../components/details/ApplicationInformation";
import WorkflowTimeline from "../components/details/WorkflowTimeline";
import ApplicationDetailsSkeleton from "../components/details/ApplicationDetailsSkeleton";
import ApplicationDetailsError from "../components/details/ApplicationDetailsError";

import ScreeningDialog from "../../hr/components/applications/dialogs/ScreeningDialog";
import ScheduleInterviewDialog from "../../hr/components/applications/dialogs/ScheduleInterviewDialog";
import SelectionDecisionDialog from "../../hr/components/applications/dialogs/SelectionDecisionDialog";

import { useApplication } from "../hooks/useApplication";

export default function ApplicationDetailsPage() {
  const { id = "" } = useParams();
  const navigate = useNavigate();

  const { application, loading, error, refetch } = useApplication(id);

  const [screeningOpen, setScreeningOpen] = useState(false);
  const [interviewOpen, setInterviewOpen] = useState(false);
  const [selectionOpen, setSelectionOpen] = useState(false);

  function handleSuccess() {
    refetch();
    setScreeningOpen(false);
    setInterviewOpen(false);
    setSelectionOpen(false);
  }

  if (loading) {
    return <ApplicationDetailsSkeleton />;
  }

  if (error || !application) {
    return (
      <ApplicationDetailsError
        message={error ?? "Unable to load application details."}
        onRetry={refetch}
      />
    );
  }

  return (
    <>
      <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
        <ApplicationActions
          application={application}
          onBack={() => navigate(-1)}
          onScreening={() => setScreeningOpen(true)}
          onInterview={() => setInterviewOpen(true)}
          onSelection={() => setSelectionOpen(true)}
        />

        <ApplicationHeader application={application} />

        <div className="grid gap-6 lg:grid-cols-2">
          <CandidateInformation application={application} />
          <JobInformation application={application} />
        </div>

        <ApplicationInformation application={application} />

        <WorkflowTimeline status={application.status} />
      </div>

      <ScreeningDialog
        open={screeningOpen}
        applicationId={application.id}
        onClose={() => setScreeningOpen(false)}
        onSuccess={handleSuccess}
      />

      <ScheduleInterviewDialog
        open={interviewOpen}
        applicationId={application.id}
        interviewers={[]} // Note: Pass real interviewers here
        onClose={() => setInterviewOpen(false)}
        onSuccess={handleSuccess}
      />

      <SelectionDecisionDialog
        open={selectionOpen}
        applicationId={application.id}
        onClose={() => setSelectionOpen(false)}
        onSuccess={handleSuccess}
      />
    </>
  );
}