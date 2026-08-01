import { Users } from "lucide-react";

export default function EmployeeEmpty() {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white px-6 py-20 text-center shadow-sm">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
        <Users size={32} />
      </div>
      <h3 className="text-xl font-bold text-[#212529]">
        No employees found
      </h3>
      <p className="mt-2 text-sm font-medium text-[#75837D]">
        Try adjusting your search criteria or filter to find what you're looking for.
      </p>
    </div>
  );
}