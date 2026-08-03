export default function CandidateProfileSkeleton() {
  return (
    <div className="space-y-6 animate-pulse">
      {/* Header */}
      <div className="h-44 rounded-2xl bg-[#EEF3F0]" />

      <div className="grid gap-6 lg:grid-cols-3">
        {/* Left */}
        <div className="space-y-6 lg:col-span-1">
          <div className="h-72 rounded-2xl bg-[#EEF3F0]" />
          <div className="h-60 rounded-2xl bg-[#EEF3F0]" />
        </div>

        {/* Right */}
        <div className="space-y-6 lg:col-span-2">
          <div className="h-56 rounded-2xl bg-[#EEF3F0]" />
          <div className="h-72 rounded-2xl bg-[#EEF3F0]" />
          <div className="h-72 rounded-2xl bg-[#EEF3F0]" />
        </div>
      </div>
    </div>
  );
}