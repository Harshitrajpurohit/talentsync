import { useRef, useState } from "react";
import {
  FileText,
  UploadCloud,
  Download,
  RefreshCw,
  Loader2,
} from "lucide-react";


import { useUploadResume } from "../hooks/useUploadResume";
import { useReplaceResume } from "../hooks/useReplaceResume";
import type { Resume } from "../types/resume";

function formatFileSize(bytes: number) {
  if (bytes < 1024) return `${bytes} B`;
  if (bytes < 1024 * 1024) return `${(bytes / 1024).toFixed(1)} KB`;
  return `${(bytes / (1024 * 1024)).toFixed(1)} MB`;
}

interface Props {
  resume: Resume | null;
  loading: boolean;
  refresh: () => Promise<void>;
}

export default function ResumeCard({
  resume,
  loading,
  refresh,
}: Props) {
  
  const { upload, loading: uploading } = useUploadResume();
  const { replace, loading: replacing } = useReplaceResume();

  const [selectedFile, setSelectedFile] = useState<File | null>(null);

  const fileInputRef = useRef<HTMLInputElement>(null);

  const isBusy = uploading || replacing;

  async function handleAction() {
    if (!selectedFile) return;

    if (selectedFile.type !== "application/pdf") {
      alert("Only PDF files are allowed.");
      return;
    }

    if (selectedFile.size > 5 * 1024 * 1024) {
      alert("Maximum file size is 5 MB.");
      return;
    }

    if (resume) {
      await replace(resume.id, selectedFile);
    } else {
      await upload(selectedFile);
    }

    setSelectedFile(null);

    if (fileInputRef.current) {
      fileInputRef.current.value = "";
    }

    await refresh();
  }

  if (loading) {
    return (
      <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
        <div className="flex justify-center py-8">
          <Loader2 className="animate-spin text-[#315343]" />
        </div>
      </div>
    );
  }

  return (
    <div className="rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm">
      <h3 className="mb-5 text-lg font-bold text-[#212529]">
        Resume
      </h3>

      {/* Upload Area */}
      <div
        className="cursor-pointer rounded-xl border-2 border-dashed border-[#E5EAE7] bg-[#EEF3F0]/50 p-6 text-center transition hover:border-[#315343] hover:bg-[#EEF3F0]"
        onClick={() => fileInputRef.current?.click()}
      >
        <div className="mx-auto mb-3 flex h-12 w-12 items-center justify-center rounded-full bg-white shadow-sm">
          <UploadCloud className="text-[#315343]" />
        </div>

        <p className="font-semibold text-[#212529]">
          {resume ? "Choose New Resume" : "Upload Resume"}
        </p>

        <p className="mt-1 text-sm text-[#75837D]">
          PDF only • Maximum 5 MB
        </p>

        <input
          ref={fileInputRef}
          type="file"
          accept=".pdf"
          hidden
          onChange={(e) =>
            setSelectedFile(e.target.files?.[0] ?? null)
          }
        />
      </div>

      {selectedFile && (
        <div className="mt-4 rounded-lg bg-[#EEF3F0] p-3 text-sm">
          Selected: <strong>{selectedFile.name}</strong>
        </div>
      )}

      {resume && (
        <div className="mt-5 rounded-xl border border-[#E5EAE7] p-4">
          <div className="flex items-center gap-3">
            <div className="flex h-10 w-10 items-center justify-center rounded-lg bg-red-100 text-red-600">
              <FileText size={20} />
            </div>

            <div className="min-w-0 flex-1">
              <p className="truncate font-semibold text-[#212529]">
                {resume.fileName}
              </p>

              <p className="text-sm text-[#75837D]">
                {formatFileSize(resume.fileSize)}
              </p>

              <p className="text-xs text-[#75837D]">
                Uploaded{" "}
                {new Date(resume.uploadedDate).toLocaleDateString()}
              </p>
            </div>

            <a
              href={resume.fileUrl}
              target="_blank"
              rel="noreferrer"
              className="rounded-lg border border-[#E5EAE7] p-2 transition hover:bg-[#EEF3F0]"
            >
              <Download size={18} />
            </a>
          </div>
        </div>
      )}

      {selectedFile && (
        <button
          onClick={handleAction}
          disabled={isBusy}
          className="mt-5 flex w-full items-center justify-center gap-2 rounded-xl bg-[#315343] px-4 py-3 font-semibold text-white transition hover:bg-[#274235] disabled:opacity-60"
        >
          {isBusy ? (
            <Loader2 className="h-4 w-4 animate-spin" />
          ) : (
            <RefreshCw className="h-4 w-4" />
          )}

          {resume ? "Replace Resume" : "Upload Resume"}
        </button>
      )}
    </div>
  );
}