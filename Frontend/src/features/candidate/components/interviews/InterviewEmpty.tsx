import { CalendarX2 } from "lucide-react";

export default function InterviewEmpty() {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white px-8 py-20 text-center shadow-sm">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
        <CalendarX2 size={32} />
      </div>

      <h2 className="text-xl font-bold text-[#212529]">
        No Interviews Found
      </h2>

      <p className="mt-2 text-sm font-medium text-[#75837D]">
        You don't have any interviews matching the selected filters.
      </p>
    </div>
  );
}