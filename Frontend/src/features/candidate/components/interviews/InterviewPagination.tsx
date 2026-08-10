interface InterviewPaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

export default function InterviewPagination({
  currentPage,
  totalPages,
  onPageChange,
}: InterviewPaginationProps) {
  if (totalPages <= 1) return null;

  return (
    <div className="flex items-center justify-between rounded-2xl border border-[#E5EAE7] bg-white px-6 py-4 shadow-sm">
      <button
        onClick={() => onPageChange(currentPage - 1)}
        disabled={currentPage === 1}
        className="rounded-[10px] border border-[#E5EAE7] px-5 py-2 text-sm font-bold text-[#212529] transition hover:bg-[#EEF3F0] hover:text-[#315343] active:scale-95 disabled:pointer-events-none disabled:opacity-40"
      >
        Previous
      </button>

      <span className="text-sm font-bold text-[#75837D]">
        Page <span className="text-[#212529]">{currentPage}</span> of {totalPages}
      </span>

      <button
        onClick={() => onPageChange(currentPage + 1)}
        disabled={currentPage === totalPages}
        className="rounded-[10px] border border-[#E5EAE7] px-5 py-2 text-sm font-bold text-[#212529] transition hover:bg-[#EEF3F0] hover:text-[#315343] active:scale-95 disabled:pointer-events-none disabled:opacity-40"
      >
        Next
      </button>
    </div>
  );
}