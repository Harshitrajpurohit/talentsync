export default function CandidateSkeleton() {
  return (
    <div className="overflow-hidden rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="animate-pulse">
        <div className="h-14 bg-[#F8FAF9] dark:bg-[#1e3329]" />

        {[...Array(8)].map((_, index) => (
          <div
            key={index}
            className="flex items-center gap-6 border-t border-[#E5EAE7] px-6 py-5 dark:border-[#315343]"
          >
            <div className="h-10 w-10 rounded-full bg-[#E5EAE7] dark:bg-[#1e3329]" />

            <div className="flex-1 space-y-2.5">
              <div className="h-3.5 w-48 rounded-full bg-[#E5EAE7] dark:bg-[#1e3329]" />
              <div className="h-2.5 w-64 rounded-full bg-[#E5EAE7] dark:bg-[#1e3329]" />
            </div>

            <div className="h-7 w-20 rounded-full bg-[#E5EAE7] dark:bg-[#1e3329]" />

            <div className="h-9 w-24 rounded-[12px] bg-[#E5EAE7] dark:bg-[#1e3329]" />
          </div>
        ))}
      </div>
    </div>
  );
}