interface JobPaginationProps {
  pageNumber: number;
  pageSize: number;
  totalRecords: number;
  onPageChange: (page: number) => void;
}

export default function JobPagination({
  pageNumber,
  pageSize,
  totalRecords,
  onPageChange,
}: JobPaginationProps) {
  const totalPages = Math.ceil(totalRecords / pageSize);

  if (totalPages <= 1) {
    return null;
  }

  return (
    <div className="flex items-center justify-between rounded-[20px] border border-[#E5EAE7] bg-white p-4 shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <button
        disabled={pageNumber === 1}
        onClick={() => onPageChange(pageNumber - 1)}
        className="rounded-[20px] border border-[#E5EAE7] px-4 py-2 text-sm font-medium text-[#212529] transition-all duration-300 hover:border-[#315343] hover:bg-[#315343] hover:text-[#C3F53C] disabled:cursor-not-allowed disabled:border-[#E5EAE7] disabled:bg-transparent disabled:text-[#75837D] disabled:opacity-50 dark:border-gray-700"
      >
        Previous
      </button>

      <span className="text-sm font-medium text-[#75837D] dark:text-gray-300">
        Page {pageNumber} of {totalPages}
      </span>

      <button
        disabled={pageNumber === totalPages}
        onClick={() => onPageChange(pageNumber + 1)}
        className="rounded-[20px] border border-[#E5EAE7] px-4 py-2 text-sm font-medium text-[#212529] transition-all duration-300 hover:border-[#315343] hover:bg-[#315343] hover:text-[#C3F53C] disabled:cursor-not-allowed disabled:border-[#E5EAE7] disabled:bg-transparent disabled:text-[#75837D] disabled:opacity-50 dark:border-gray-700"
      >
        Next
      </button>
    </div>
  );
}