import { CalendarDays, Search } from "lucide-react";

interface InterviewEmptyProps {
  filtered?: boolean;
}

export default function InterviewEmpty({ filtered = false }: InterviewEmptyProps) {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white px-8 py-20 text-center shadow-sm">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
        {filtered ? <Search size={32} /> : <CalendarDays size={32} />}
      </div>

      <h3 className="text-xl font-bold text-[#212529]">
        {filtered ? "No interviews found" : "No interviews assigned"}
      </h3>

      <p className="mt-2 max-w-md text-sm font-medium text-[#75837D]">
        {filtered
          ? "Try adjusting your search or filter criteria to find what you're looking for."
          : "You currently have no interviews assigned to you."}
      </p>
    </div>
  );
}