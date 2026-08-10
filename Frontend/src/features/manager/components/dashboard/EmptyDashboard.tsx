import { LayoutDashboard } from "lucide-react";

export default function EmptyDashboard() {
  return (
    <div className="flex min-h-[400px] items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white shadow-sm">
      <div className="text-center">
        <div className="mx-auto flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0]">
          <LayoutDashboard size={32} className="text-[#315343]" />
        </div>

        <h2 className="mt-5 text-xl font-bold text-[#212529]">
          No Dashboard Data
        </h2>

        <p className="mx-auto mt-2 max-w-md text-sm font-medium leading-relaxed text-[#75837D]">
          There is currently no recruitment data available for your dashboard.
          Try posting a new job to get started.
        </p>
      </div>
    </div>
  );
}