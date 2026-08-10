export default function DashboardSkeleton() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="overflow-hidden rounded-2xl bg-[#315343] px-6 py-8 shadow-sm">
        <div className="animate-pulse">
          <div className="h-6 w-32 rounded bg-white/20" />
          <div className="mt-4 h-8 w-72 rounded bg-white/20" />
          <div className="mt-2 h-4 w-96 max-w-full rounded bg-white/20" />
        </div>
      </div>

      {/* Summary */}
      <div className="grid grid-cols-2 gap-4 lg:grid-cols-3 xl:grid-cols-6">
        {Array.from({ length: 6 }).map((_, index) => (
          <div
            key={index}
            className="h-28 animate-pulse rounded-2xl bg-[#EEF3F0]"
          />
        ))}
      </div>

      {/* Main sections */}
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-3 xl:gap-6">
        <div className="flex flex-col space-y-6 xl:col-span-2">
          <div className="h-96 animate-pulse rounded-2xl bg-[#EEF3F0]" />
          <div className="h-80 animate-pulse rounded-2xl bg-[#EEF3F0]" />
        </div>

        <div className="xl:col-span-1">
          <div className="h-[500px] animate-pulse rounded-2xl bg-[#EEF3F0]" />
        </div>
      </div>
    </div>
  );
}