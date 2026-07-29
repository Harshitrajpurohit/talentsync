import { Loader2 } from "lucide-react";

interface ApplyJobButtonProps {
  hasApplied: boolean;
  loading: boolean;
  onApply: () => void;
}

export default function ApplyJobButton({
  hasApplied,
  loading,
  onApply,
}: ApplyJobButtonProps) {
  return (
    <button
      disabled={hasApplied || loading}
      onClick={onApply}
      className="group flex w-full items-center justify-center rounded-[20px] bg-[#315343] px-5 py-3 text-sm font-semibold text-white shadow-sm transition-all duration-300 hover:bg-[#C3F53C] hover:text-[#315343] hover:shadow-md disabled:cursor-not-allowed disabled:bg-[#E5EAE7] disabled:text-[#75837D] disabled:hover:shadow-sm"
    >
      {loading ? (
        <>
          {/* The loader inherits the vivid accent, then flips to the dark brand color on hover */}
          <Loader2
            size={18}
            className="mr-2 animate-spin text-[#C3F53C] transition-colors duration-300 group-hover:text-[#315343] group-disabled:text-[#75837D]"
          />
          Applying...
        </>
      ) : hasApplied ? (
        "Already Applied"
      ) : (
        "Apply Now"
      )}
    </button>
  );
}