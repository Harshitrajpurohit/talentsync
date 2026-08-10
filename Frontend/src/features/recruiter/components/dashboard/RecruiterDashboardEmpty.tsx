import { BriefcaseBusiness } from "lucide-react";

interface RecruiterDashboardEmptyProps {
  message?: string;
}

export default function RecruiterDashboardEmpty({
  message = "No dashboard data available.",
}: RecruiterDashboardEmptyProps) {
  return (
    <div className="flex min-h-[450px] items-center justify-center rounded-[20px] border border-dashed border-[#E5EAE7] bg-white">
      <div className="flex max-w-sm flex-col items-center text-center">
        <div className="flex h-20 w-20 items-center justify-center rounded-full bg-[#EEF3F0]">
          <BriefcaseBusiness
            size={40}
            className="text-[#315343]"
          />
        </div>

        <h2 className="mt-6 text-xl font-bold text-[#212529]">
          Recruiter Dashboard
        </h2>

        <p className="mt-2 text-sm leading-relaxed text-[#75837D]">
          {message}
        </p>
      </div>
    </div>
  );
}