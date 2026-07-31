import {
  Download,
  FileText,
  CalendarDays,
  HardDrive,
} from "lucide-react";

import type { Resume } from "../../../types/resume";

interface CandidateResumeCardProps {
  resume?: Resume;
  loading?: boolean;
}

function formatFileSize(bytes: number) {
  if (bytes < 1024) return `${bytes} B`;

  if (bytes < 1024 * 1024) {
    return `${(bytes / 1024).toFixed(1)} KB`;
  }

  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

export default function CandidateResumeCard({
  resume,
  loading = false,
}: CandidateResumeCardProps) {

  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">

        <div className="mb-5 h-6 w-32 animate-pulse rounded bg-gray-200 dark:bg-gray-700" />

        <div className="space-y-4">

          <div className="h-14 animate-pulse rounded-xl bg-gray-100 dark:bg-gray-700" />

          <div className="h-5 w-40 animate-pulse rounded bg-gray-100 dark:bg-gray-700" />

          <div className="h-5 w-52 animate-pulse rounded bg-gray-100 dark:bg-gray-700" />

          <div className="h-10 w-44 animate-pulse rounded-xl bg-gray-100 dark:bg-gray-700" />

        </div>

      </div>
    );
  }


  if (!resume) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">

        <h2 className="mb-5 text-lg font-semibold dark:text-white">
          Resume
        </h2>

        <div className="flex flex-col items-center justify-center py-8 text-center">

          <FileText className="mb-3 h-12 w-12 text-[#75837D]" />

          <p className="font-medium dark:text-white">
            No Resume Uploaded
          </p>

          <p className="mt-1 text-sm text-[#75837D]">
            Candidate has not uploaded a resume yet.
          </p>

        </div>

      </div>
    );
  }


  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">

      <h2 className="mb-5 text-lg font-semibold dark:text-white">
        Resume
      </h2>


      <div className="space-y-4">

        <div className="flex items-center gap-3">

          <FileText className="text-[#315343]" />

          <div>
            <p className="font-medium dark:text-white">
              {resume.fileName}
            </p>

            <p className="text-sm text-[#75837D]">
              {resume.contentType}
            </p>
          </div>

        </div>


        <div className="flex items-center gap-2 text-sm text-[#75837D]">
          <HardDrive size={16} />
          {formatFileSize(resume.fileSize)}
        </div>


        <div className="flex items-center gap-2 text-sm text-[#75837D]">
          <CalendarDays size={16} />

          Uploaded{" "}
          {new Date(resume.uploadedDate).toLocaleDateString()}
        </div>


        <a
          href={resume.fileUrl}
          target="_blank"
          rel="noreferrer"
          className="mt-4 inline-flex items-center gap-2 rounded-xl bg-[#315343] px-4 py-2 text-white transition hover:bg-[#274236]"
        >
          <Download size={18} />
          Download Resume
        </a>

      </div>

    </div>
  );
}