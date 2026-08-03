export default function ApplicationDetailsSkeleton() {
  return (
    <div className="space-y-6 animate-pulse">
      <div className="flex justify-end gap-3">
        <div className="h-10 w-28 rounded-full bg-[#EEF3F0]" />
        <div className="h-10 w-40 rounded-full bg-[#EEF3F0]" />
      </div>

      <div className="h-28 rounded-2xl bg-[#EEF3F0]" />

      <div className="grid gap-6 lg:grid-cols-2">
        <div className="h-48 rounded-2xl bg-[#EEF3F0]" />
        <div className="h-48 rounded-2xl bg-[#EEF3F0]" />
      </div>

      <div className="h-56 rounded-2xl bg-[#EEF3F0]" />
      <div className="h-44 rounded-2xl bg-[#EEF3F0]" />
    </div>
  );
}