import { Users } from "lucide-react";

export default function CandidateEmpty() {
  return (
    <div className="flex flex-col items-center justify-center rounded-[20px] border-2 border-dashed border-[#E5EAE7] bg-white px-8 py-16 text-center shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="flex h-20 w-20 items-center justify-center rounded-full bg-[#F8FAF9] dark:bg-[#1e3329]">
        <Users className="h-10 w-10 text-[#315343] dark:text-[#C3F53C]" />
      </div>

      <h2 className="mt-5 text-lg font-bold text-[#212529] dark:text-white">
        No Candidates Found
      </h2>

      <p className="mt-2 max-w-md text-sm text-[#75837D] dark:text-white/60">
        There are no candidates matching your current search or filters.
      </p>
    </div>
  );
}