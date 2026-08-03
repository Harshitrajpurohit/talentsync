export default function ApplicationSkeleton() {
  return (
    <div className="overflow-hidden rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      <div className="animate-pulse">
        <div className="h-14 border-b border-[#E5EAE7] bg-[#F8FAF9]" />

        {Array.from({ length: 6 }).map((_, index) => (
          <div
            key={index}
            className="flex items-center gap-6 border-b border-[#E5EAE7] px-6 py-5 last:border-0"
          >
            <div className="h-4 w-40 rounded bg-[#EEF3F0]" />
            <div className="h-4 w-36 rounded bg-[#EEF3F0]" />
            <div className="h-4 w-28 rounded bg-[#EEF3F0]" />
            <div className="h-6 w-24 rounded-full bg-[#EEF3F0]" />
            <div className="ml-auto h-8 w-24 rounded-full bg-[#EEF3F0]" />
          </div>
        ))}
      </div>
    </div>
  );
}