export default function EmployeeSkeleton() {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="border-b border-[#E5EAE7] bg-[#F8FAF9] px-6 py-4">
        <div className="h-5 w-full max-w-4xl rounded bg-[#EEF3F0]" />
      </div>
      <div className="animate-pulse">
        {Array.from({ length: 6 }).map((_, index) => (
          <div
            key={index}
            className="flex items-center gap-6 border-b border-[#E5EAE7] px-6 py-5 last:border-0"
          >
            <div className="h-10 w-10 shrink-0 rounded-full bg-[#EEF3F0]" />
            <div className="flex-1 space-y-2.5">
              <div className="h-4 w-40 rounded bg-[#EEF3F0]" />
              <div className="h-3 w-56 rounded bg-[#EEF3F0]" />
            </div>
            <div className="h-4 w-24 rounded bg-[#EEF3F0]" />
            <div className="h-4 w-28 rounded bg-[#EEF3F0]" />
            <div className="h-4 w-28 rounded bg-[#EEF3F0]" />
            <div className="h-8 w-20 rounded-full bg-[#EEF3F0]" />
          </div>
        ))}
      </div>
    </div>
  );
}