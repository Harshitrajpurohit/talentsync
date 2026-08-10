export default function RecruiterDashboardSkeleton() {
  return (
    <div className="space-y-6 animate-pulse">
      {/* Header */}
      <div className="h-36 rounded-[20px] bg-[#EEF3F0]" />

      {/* Stats */}
      <div className="grid gap-5 sm:grid-cols-2 xl:grid-cols-3">
        {Array.from({ length: 6 }).map((_, index) => (
          <div
            key={index}
            className="h-32 rounded-[20px] bg-[#EEF3F0]"
          />
        ))}
      </div>

      {/* Cards */}
      <div className="grid gap-6 xl:grid-cols-2">
        {Array.from({ length: 2 }).map((_, index) => (
          <div
            key={index}
            className="rounded-[20px] border border-[#E5EAE7] bg-white p-6"
          >
            <div className="mb-6 h-6 w-40 rounded bg-[#EEF3F0]" />

            <div className="space-y-4">
              {Array.from({ length: 5 }).map((__, row) => (
                <div
                  key={row}
                  className="flex items-center justify-between"
                >
                  <div className="space-y-2">
                    <div className="h-4 w-36 rounded bg-[#EEF3F0]" />
                    <div className="h-3 w-24 rounded bg-[#EEF3F0]" />
                  </div>

                  <div className="h-8 w-20 rounded-full bg-[#EEF3F0]" />
                </div>
              ))}
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}