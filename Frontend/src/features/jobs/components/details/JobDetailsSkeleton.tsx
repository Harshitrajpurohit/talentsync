export default function JobDetailsSkeleton() {
  return (
    <div className="space-y-6 animate-pulse">
      {/* Header Banner */}
      <div className="h-40 rounded-2xl bg-[#EEF3F0] dark:bg-[#315343]" />

      {/* Top Metrics Row */}
      <div className="grid gap-5 sm:grid-cols-2 xl:grid-cols-3">
        {[...Array(3)].map((_, index) => (
          <div
            key={index}
            className="h-32 rounded-2xl bg-[#EEF3F0] dark:bg-[#315343]"
          />
        ))}
      </div>

      {/* Two Column Section */}
      <div className="grid gap-6 lg:grid-cols-2">
        <div className="h-64 rounded-2xl bg-[#EEF3F0] dark:bg-[#315343]" />
        <div className="h-64 rounded-2xl bg-[#EEF3F0] dark:bg-[#315343]" />
      </div>

      {/* Bottom Full Width Section */}
      <div className="h-96 rounded-2xl bg-[#EEF3F0] dark:bg-[#315343]" />
    </div>
  );
}