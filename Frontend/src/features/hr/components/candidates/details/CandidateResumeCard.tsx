import { Download, FileText, CalendarDays, HardDrive } from "lucide-react";
import type { Resume } from "../../../types/resume";

interface CandidateResumeCardProps {
  resume?: Resume;
  loading?: boolean;
}

function formatFileSize(bytes: number) {
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

export default function CandidateResumeCard({
  resume,
  loading = false,
}: CandidateResumeCardProps) {
  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
        <div className="mb-5 h-6 w-32 animate-pulse rounded bg-[#EEF3F0]" />
        <div className="space-y-4">
          <div className="h-14 animate-pulse rounded-xl bg-[#EEF3F0]" />
          <div className="h-5 w-40 animate-pulse rounded bg-[#EEF3F0]" />
          <div className="h-10 w-44 animate-pulse rounded-xl bg-[#EEF3F0]" />
        </div>
      </div>
    );
  }

  if (!resume) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
        <h2 className="mb-5 text-lg font-bold text-[#212529]">Resume</h2>
        <div className="flex flex-col items-center justify-center rounded-xl border-2 border-dashed border-[#E5EAE7] bg-[#F8FAF9] py-10 text-center">
          <FileText className="mb-3 h-10 w-10 text-[#75837D]" />
          <p className="text-sm font-bold text-[#212529]">No Resume Uploaded</p>
          <p className="mt-1 text-xs font-medium text-[#75837D]">
            Candidate has not provided a resume document.
          </p>
        </div>
      </div>
    );
  }

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
      <h2 className="mb-5 text-lg font-bold text-[#212529]">Resume</h2>

      <div className="space-y-4">
        <div className="flex items-center gap-3 rounded-xl border border-[#E5EAE7] p-4 bg-[#F8FAF9]">
          <div className="flex h-10 w-10 shrink-0 items-center justify-center rounded-lg bg-red-100 text-red-600">
             <FileText size={20} />
          </div>
          <div className="min-w-0 flex-1">
            <p className="truncate text-sm font-bold text-[#212529]">
              {resume.fileName}
            </p>
            <p className="text-xs font-medium text-[#75837D]">
              {resume.contentType}
            </p>
          </div>
        </div>

        <div className="flex items-center gap-6 text-sm font-medium text-[#75837D]">
          <div className="flex items-center gap-2">
            <HardDrive size={16} className="text-[#315343]" />
            {formatFileSize(resume.fileSize)}
          </div>
          <div className="flex items-center gap-2">
            <CalendarDays size={16} className="text-[#315343]" />
            {new Date(resume.uploadedDate).toLocaleDateString()}
          </div>
        </div>

        <a
          href={resume.fileUrl}
          target="_blank"
          rel="noreferrer"
          className="mt-2 inline-flex items-center gap-2 rounded-xl bg-[#315343] px-5 py-2.5 text-sm font-bold text-white transition-all hover:bg-[#C3F53C] hover:text-[#315343] active:scale-95 shadow-sm"
        >
          <Download size={18} />
          Download Resume
        </a>
      </div>
    </div>
  );
}