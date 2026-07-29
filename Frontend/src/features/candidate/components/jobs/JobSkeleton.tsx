export default function JobSkeleton() {
  return (
    <div className="animate-pulse rounded-[20px] border border-[#E5EAE7] bg-white p-5 shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <div className="mb-4 h-6 w-1/2 rounded bg-[#E5EAE7] dark:bg-gray-700" />
      <div className="mb-3 h-4 w-1/3 rounded bg-[#E5EAE7] dark:bg-gray-800" />
      <div className="mb-6 h-4 w-1/4 rounded bg-[#E5EAE7] dark:bg-gray-800" />

      <div className="flex justify-between">
        <div className="h-8 w-20 rounded bg-[#E5EAE7] dark:bg-gray-800" />
        <div className="h-10 w-28 rounded-lg bg-[#E5EAE7] dark:bg-gray-700" />
      </div>
    </div>
  );
}