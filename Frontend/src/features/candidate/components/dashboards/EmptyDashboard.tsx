import { SearchX } from "lucide-react";
import { Link } from "react-router-dom";

export default function EmptyDashboard() {
  return (
    <div className="flex min-h-[50vh] flex-col items-center justify-center rounded-[20px] border-2 border-dashed border-[#E5EAE7] bg-white p-8 text-center shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      
      <div className="mb-5 flex h-20 w-20 items-center justify-center rounded-full bg-[#F8FAF9] dark:bg-[#1e3329]">
        <SearchX
          size={40}
          className="text-[#315343] dark:text-[#C3F53C]"
          strokeWidth={1.5}
        />
      </div>

      <h2 className="text-xl font-bold tracking-tight text-[#212529] dark:text-white">
        No Dashboard Data Available
      </h2>

      <p className="mt-2.5 max-w-sm text-sm leading-relaxed text-[#75837D] dark:text-white/60">
        Complete your profile and start applying for jobs to see your dashboard
        statistics and activity.
      </p>

      <Link
        to="/candidate/profile"
        className="group mt-6 inline-flex items-center justify-center rounded-[14px] bg-[#315343] px-7 py-3 text-sm font-bold text-[#C3F53C] shadow-sm transition-all duration-300 hover:-translate-y-0.5 hover:bg-[#253f33] hover:shadow-md dark:bg-[#C3F53C] dark:text-[#315343] dark:hover:bg-[#b0df35]"
      >
        Complete Profile
      </Link>
      
    </div>
  );
}