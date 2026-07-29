import { BriefcaseBusiness } from "lucide-react";

export default function EmptyApplications() {
  return (
    <div className="group rounded-[20px] border-2 border-dashed border-[#E5EAE7] bg-white py-16 text-center transition-all duration-300 hover:border-[#315343] hover:bg-gray-50/50 dark:border-gray-700 dark:bg-gray-900">
      <BriefcaseBusiness
        size={48}
        className="mx-auto text-[#75837D] transition-colors duration-300 group-hover:text-[#C3F53C]"
      />

      <h2 className="mt-4 text-xl font-semibold text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
        No Applications Found
      </h2>

      <p className="mt-2 text-sm text-[#75837D]">
        You haven't applied to any jobs yet.
      </p>
    </div>
  );
}