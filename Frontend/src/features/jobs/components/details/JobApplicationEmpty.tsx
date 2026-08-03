import { Inbox } from "lucide-react";

export default function JobApplicationEmpty() {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white px-6 py-20 text-center shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343] dark:bg-[#1E3329] dark:text-[#C3F53C]">
        <Inbox size={32} />
      </div>
      <h3 className="text-xl font-bold text-[#212529] dark:text-white">
        No applications yet
      </h3>
      <p className="mt-2 text-sm font-medium text-[#75837D] dark:text-white/70">
        Candidates who apply for this job will appear here.
      </p>
    </div>
  );
}