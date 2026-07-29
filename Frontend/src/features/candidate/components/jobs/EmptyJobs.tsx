import { BriefcaseBusiness } from "lucide-react";

export default function EmptyJobs() {
  return (
    <div className="group rounded-[20px] border-2 border-dashed border-[#E5EAE7] bg-white py-16 text-center transition-all duration-300 hover:border-[#315343] hover:bg-gray-50/50 dark:border-gray-700 dark:bg-gray-900">
      <BriefcaseBusiness
        size={52}
        className="mx-auto mb-4 text-[#75837D] transition-colors duration-300 group-hover:text-[#C3F53C]"
      />

      <h3 className="text-lg font-semibold text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
        No jobs found
      </h3>

      <p className="mt-2 text-sm text-[#75837D]">
        Try changing your search or filters.
      </p>
    </div>
  );
}