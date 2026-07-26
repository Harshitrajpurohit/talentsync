export default function ProfileSkeleton() {
  return (
    <div className="animate-pulse space-y-6">
      {/* Header Skeleton */}
      <div className="h-40 w-full rounded-2xl bg-white border border-[#E5EAE7] shadow-sm"></div>

      <div className="grid grid-cols-1 gap-6 lg:grid-cols-3">
        {/* Sidebar Skeletons */}
        <div className="space-y-6 lg:col-span-1">
          <div className="h-64 w-full rounded-2xl bg-white border border-[#E5EAE7] shadow-sm"></div>
          <div className="h-48 w-full rounded-2xl bg-white border border-[#E5EAE7] shadow-sm"></div>
        </div>

        {/* Main Content Skeleton */}
        <div className="space-y-6 lg:col-span-2">
          <div className="h-96 w-full rounded-2xl bg-white border border-[#E5EAE7] shadow-sm"></div>
        </div>
      </div>
    </div>
  );
}