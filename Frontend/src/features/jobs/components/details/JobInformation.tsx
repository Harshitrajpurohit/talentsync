import type { JobDetails } from "../../types/job";

interface JobInformationProps {
  job: JobDetails;
}

export default function JobInformation({ job }: JobInformationProps) {
  return (
    <div className="grid gap-6 lg:grid-cols-2">
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
        <h2 className="mb-4 text-lg font-bold text-[#212529]">
          Description
        </h2>
        <div className="prose prose-sm max-w-none text-[#75837D]">
          <p className="whitespace-pre-wrap leading-relaxed">
            {job.description}
          </p>
        </div>
      </div>

      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm sm:p-8">
        <h2 className="mb-4 text-lg font-bold text-[#212529]">
          Requirements
        </h2>
        <div className="prose prose-sm max-w-none text-[#75837D]">
          <p className="whitespace-pre-wrap leading-relaxed">
            {job.requirements}
          </p>
        </div>
      </div>
    </div>
  );
}