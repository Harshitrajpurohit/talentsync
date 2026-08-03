import { CheckCircle2, Circle } from "lucide-react";
import type { ApplicationStatus } from "../../../../shared/types/recruitment";

interface WorkflowTimelineProps {
  status: ApplicationStatus;
}

const workflow: ApplicationStatus[] = [
  "Submitted",
  "Screening",
  "InterviewScheduled",
  "InterviewCompleted",
  "Selected",
];

export default function WorkflowTimeline({ status }: WorkflowTimelineProps) {
  // If the status is Rejected, we'll just base the timeline progress on where they failed.
  // For a strict visualization, we'll mark progress up to the last successful stage.
  const currentIndex = status === "Rejected" ? -1 : workflow.indexOf(status);

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
      <h2 className="mb-6 text-lg font-bold text-[#212529]">
        Recruitment Workflow
      </h2>

      {status === "Rejected" ? (
        <div className="rounded-xl border border-red-200 bg-red-50 p-4 text-center">
          <p className="font-bold text-red-700">Application Rejected</p>
          <p className="mt-1 text-sm font-medium text-red-600">The workflow for this application has concluded.</p>
        </div>
      ) : (
        <div className="flex flex-wrap gap-3">
          {workflow.map((step, index) => {
            const completed = index <= currentIndex;

            return (
              <div
                key={step}
                className={`flex items-center gap-2 rounded-full border px-4 py-2 transition-colors ${
                  completed
                    ? "border-[#315343] bg-[#315343] text-[#C3F53C]"
                    : "border-[#E5EAE7] bg-[#EEF3F0] text-[#75837D]"
                }`}
              >
                {completed ? <CheckCircle2 size={16} /> : <Circle size={16} />}
                <span className="text-xs font-bold uppercase tracking-wider">
                  {step.replace(/([A-Z])/g, " $1").trim()}
                </span>
              </div>
            );
          })}
        </div>
      )}
    </div>
  );
}