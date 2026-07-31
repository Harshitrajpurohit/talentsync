export default function DashboardSkeleton() {
  return (
    <div className="space-y-6 animate-pulse">
      <div className="grid gap-5 md:grid-cols-2 xl:grid-cols-5">
        {Array.from({ length: 5 }).map((_, index) => (
          <div
            key={index}
            className="h-28 rounded-[20px] bg-[#E5EAE7] dark:bg-gray-700"
          />
        ))}
      </div>

      <div className="h-72 rounded-[20px] bg-[#E5EAE7] dark:bg-gray-700" />
      <div className="h-72 rounded-[20px] bg-[#E5EAE7] dark:bg-gray-700" />
      <div className="h-72 rounded-[20px] bg-[#E5EAE7] dark:bg-gray-700" />
    </div>
  );
}