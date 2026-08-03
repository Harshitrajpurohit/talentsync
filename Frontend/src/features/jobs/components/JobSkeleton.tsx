export default function JobSkeleton() {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm dark:border-[#315343] dark:bg-[#253f33]">
      <div className="animate-pulse">
        <div className="h-14 border-b border-[#E5EAE7] bg-[#F8FAF9] dark:border-[#315343] dark:bg-[#1E3329]" />
        {Array.from({ length: 6 }).map((_, index) => (
          <div
            key={index}
            className="flex items-center justify-between border-b border-[#E5EAE7] px-6 py-5 last:border-0 dark:border-[#315343]"
          >
            <div className="space-y-2">
              <div className="h-4 w-44 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
              <div className="h-3 w-28 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
            </div>
            <div className="h-4 w-24 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="h-8 w-20 rounded-full bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="h-4 w-12 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="h-4 w-24 rounded bg-[#EEF3F0] dark:bg-[#315343]" />
            <div className="flex gap-2">
              <div className="h-8 w-8 rounded-full bg-[#EEF3F0] dark:bg-[#315343]" />
              <div className="h-8 w-8 rounded-full bg-[#EEF3F0] dark:bg-[#315343]" />
              <div className="h-8 w-8 rounded-full bg-[#EEF3F0] dark:bg-[#315343]" />
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}