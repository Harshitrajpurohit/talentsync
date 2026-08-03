import { FileSearch } from "lucide-react";

interface ApplicationEmptyProps {
  message?: string;
}

export default function ApplicationEmpty({
  message = "No applications found.",
}: ApplicationEmptyProps) {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white px-8 py-20 text-center shadow-sm">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
        <FileSearch size={32} />
      </div>

      <h3 className="text-xl font-bold text-[#212529]">
        No Applications
      </h3>

      <p className="mt-2 max-w-md text-sm font-medium text-[#75837D]">
        {message}
      </p>
    </div>
  );
}