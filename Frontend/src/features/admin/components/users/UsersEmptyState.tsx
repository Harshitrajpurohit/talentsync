import { UsersRound, SearchX } from "lucide-react";

interface UsersEmptyStateProps {
  hasFilters?: boolean;
}

export default function UsersEmptyState({ hasFilters = false }: UsersEmptyStateProps) {
  return (
    <div className="flex flex-col items-center justify-center rounded-2xl border-2 border-dashed border-[#E5EAE7] bg-white px-6 py-20 text-center shadow-sm">
      <div className="mb-4 flex h-16 w-16 items-center justify-center rounded-full bg-[#EEF3F0] text-[#315343]">
        {hasFilters ? <SearchX size={32} /> : <UsersRound size={32} />}
      </div>

      <h3 className="text-xl font-bold text-[#212529]">
        {hasFilters ? "No users found" : "No users available"}
      </h3>

      <p className="mt-2 max-w-md text-sm font-medium text-[#75837D]">
        {hasFilters
          ? "Try adjusting your search or filters to find the user you're looking for."
          : "There are currently no users available to display."}
      </p>
    </div>
  );
}