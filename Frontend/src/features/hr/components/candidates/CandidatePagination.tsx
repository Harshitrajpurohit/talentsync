interface CandidatePaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export default function CandidatePagination({
  currentPage,
  totalPages,
  onPageChange,
}: CandidatePaginationProps) {
  if (totalPages <= 1) return null;

  return (
    <div className="flex items-center justify-between rounded-[20px] border border-[#E5EAE7] bg-white p-5 shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <button
        disabled={currentPage === 1}
        onClick={() => onPageChange(currentPage - 1)}
        className="rounded-[12px] border border-[#E5EAE7] bg-white px-5 py-2 text-sm font-bold text-[#212529] transition-all duration-300 hover:border-[#315343] hover:bg-[#315343] hover:text-[#C3F53C] disabled:cursor-not-allowed disabled:opacity-50 disabled:hover:border-[#E5EAE7] disabled:hover:bg-white disabled:hover:text-[#212529] dark:border-[#315343] dark:bg-[#1e3329] dark:text-white dark:hover:border-[#C3F53C] dark:hover:bg-[#C3F53C] dark:hover:text-[#315343] dark:disabled:hover:border-[#315343] dark:disabled:hover:bg-[#1e3329] dark:disabled:hover:text-white"
      >
        Previous
      </button>

      <p className="text-sm font-bold text-[#75837D] dark:text-white/70">
        Page <span className="text-[#212529] dark:text-white">{currentPage}</span> of <span className="text-[#212529] dark:text-white">{totalPages}</span>
      </p>

      <button
        disabled={currentPage === totalPages}
        onClick={() => onPageChange(currentPage + 1)}
        className="rounded-[12px] border border-[#E5EAE7] bg-white px-5 py-2 text-sm font-bold text-[#212529] transition-all duration-300 hover:border-[#315343] hover:bg-[#315343] hover:text-[#C3F53C] disabled:cursor-not-allowed disabled:opacity-50 disabled:hover:border-[#E5EAE7] disabled:hover:bg-white disabled:hover:text-[#212529] dark:border-[#315343] dark:bg-[#1e3329] dark:text-white dark:hover:border-[#C3F53C] dark:hover:bg-[#C3F53C] dark:hover:text-[#315343] dark:disabled:hover:border-[#315343] dark:disabled:hover:bg-[#1e3329] dark:disabled:hover:text-white"
      >
        Next
      </button>
    </div>
  );
}