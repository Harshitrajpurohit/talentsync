export default function JobApplicationSkeleton() {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="animate-pulse">
        <div className="h-16 border-b border-[#E5EAE7] bg-[#F8FAF9] dark:border-[#315343] dark:bg-[#1E3329]" />
        {[...Array(6)].map((_, index) => (
          <div
            key={index}
            className="flex items-center justify-between border-b border-[#E5EAE7] px-6 py-5 last:border-0 dark:border-[#315343]"
          >
            <div className="h-5 w-32 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="h-6 w-24 rounded-md bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="h-5 w-28 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="h-8 w-20 rounded-full bg-[#EEF3F0] dark:bg-[#315343]" />
          </div>
        ))}
      </div>
    </div>
  );
}