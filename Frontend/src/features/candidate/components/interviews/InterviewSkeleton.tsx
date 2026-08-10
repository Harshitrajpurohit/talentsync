export default function InterviewSkeleton() {
  return (
    <div className="grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3">
      {Array.from({ length: 6 }).map((_, index) => (
        <div
          key={index}
          className="animate-pulse rounded-2xl border border-[#E5EAE7] bg-white p-6 shadow-sm"
        >
          <div className="mb-6 flex items-start justify-between border-b border-[#E5EAE7] pb-4">
            <div className="space-y-2">
              <div className="h-5 w-40 rounded bg-[#EEF3F0]" />
              <div className="h-3 w-24 rounded bg-[#EEF3F0]" />
            </div>
            <div className="h-6 w-20 rounded-md bg-[#EEF3F0]" />
          </div>

          <div className="space-y-4">
            <div className="flex items-center gap-3">
              <div className="h-8 w-8 shrink-0 rounded-full bg-[#EEF3F0]" />
              <div className="h-4 w-32 rounded bg-[#EEF3F0]" />
            </div>
            <div className="flex items-center gap-3">
              <div className="h-8 w-8 shrink-0 rounded-full bg-[#EEF3F0]" />
              <div className="h-4 w-24 rounded bg-[#EEF3F0]" />
            </div>
            <div className="flex items-center gap-3">
              <div className="h-8 w-8 shrink-0 rounded-full bg-[#EEF3F0]" />
              <div className="h-4 w-36 rounded bg-[#EEF3F0]" />
            </div>
            <div className="flex items-center gap-3">
              <div className="h-8 w-8 shrink-0 rounded-full bg-[#EEF3F0]" />
              <div className="h-4 w-44 rounded bg-[#EEF3F0]" />
            </div>
          </div>
        </div>
      ))}
    </div>
  );
}