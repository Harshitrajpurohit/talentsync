export default function DashboardSkeleton() {
  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="h-36 animate-pulse rounded-[20px] bg-[#E5EAE7] dark:bg-[#253F33]" />

      {/* Summary cards */}
      <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
        {[1, 2, 3, 4].map((item) => (
          <div
            key={item}
            className="h-32 animate-pulse rounded-[20px] bg-[#E5EAE7] dark:bg-[#253F33]"
          />
        ))}
      </div>

      {/* Content */}
      <div className="grid grid-cols-1 gap-6 xl:grid-cols-2">
        <div className="h-[420px] animate-pulse rounded-[20px] bg-[#E5EAE7] dark:bg-[#253F33]" />

        <div className="h-[420px] animate-pulse rounded-[20px] bg-[#E5EAE7] dark:bg-[#253F33]" />
      </div>
    </div>
  );
}