import {
  BriefcaseBusiness,
  Building2,
  CalendarDays,
  FileText,
  CheckCircle,
} from "lucide-react";
import ApplyJobButton from "./ApplyJobButton";
import type { CandidateJobDetails } from "../../types/job";

interface JobDetailsCardProps {
  job: CandidateJobDetails;
  applying: boolean;
  onApply: () => void;
}

export default function JobDetailsCard({
  job,
  applying,
  onApply,
}: JobDetailsCardProps) {
  return (
    <div className="rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <div className="space-y-6 p-6">
        <div>
          <h1 className="text-2xl font-bold text-[#212529] dark:text-white">
            {job.title}
          </h1>

          <div className="mt-3 flex flex-wrap gap-4 text-sm text-[#75837D]">
            <div className="flex items-center gap-2">
              <Building2 size={16} className="text-[#315343] dark:text-[#C3F53C]" />
              {job.department}
            </div>

            <div className="flex items-center gap-2">
              <CalendarDays size={16} className="text-[#315343] dark:text-[#C3F53C]" />
              {new Date(job.postedDate).toLocaleDateString()}
            </div>

            <div className="flex items-center gap-2">
              <BriefcaseBusiness size={16} className="text-[#315343] dark:text-[#C3F53C]" />
              {job.status}
            </div>
          </div>
        </div>

        <div>
          <div className="mb-2 flex items-center gap-2">
            <FileText size={18} className="text-[#C3F53C]" />
            <h2 className="font-semibold text-[#212529] dark:text-white">
              Description
            </h2>
          </div>

          <p className="whitespace-pre-line text-[#75837D] dark:text-gray-300">
            {job.description}
          </p>
        </div>

        <div>
          <div className="mb-2 flex items-center gap-2">
            <CheckCircle size={18} className="text-[#C3F53C]" />
            <h2 className="font-semibold text-[#212529] dark:text-white">
              Requirements
            </h2>
          </div>

          <p className="whitespace-pre-line text-[#75837D] dark:text-gray-300">
            {job.requirements}
          </p>
        </div>

        {/* This will automatically inherit your previously designed premium button! */}
        <ApplyJobButton
          hasApplied={job.hasApplied}
          loading={applying}
          onApply={onApply}
        />
      </div>
    </div>
  );
}