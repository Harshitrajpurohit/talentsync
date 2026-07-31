export default function CandidateProfileSkeleton() {
  return (
    <div className="space-y-6 animate-pulse">
      {/* Header */}
      <div className="h-44 rounded-[20px] bg-[#E5EAE7] dark:bg-[#315343]" />

      <div className="grid gap-6 lg:grid-cols-3">
        {/* Left */}
        <div className="space-y-6 lg:col-span-1">
          <div className="h-72 rounded-2xl bg-[#E5EAE7] dark:bg-[#315343]" />
          <div className="h-60 rounded-2xl bg-[#E5EAE7] dark:bg-[#315343]" />
        </div>

        {/* Right */}
        <div className="space-y-6 lg:col-span-2">
          <div className="h-56 rounded-2xl bg-[#E5EAE7] dark:bg-[#315343]" />
          <div className="h-72 rounded-2xl bg-[#E5EAE7] dark:bg-[#315343]" />
          <div className="h-72 rounded-2xl bg-[#E5EAE7] dark:bg-[#315343]" />
        </div>
      </div>
    </div>
  );
}