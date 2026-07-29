export default function ApplicationSkeleton() {
  return (
    <div className="rounded-[20px] border border-[#E5EAE7] bg-white p-6 shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <div className="animate-pulse space-y-4">
        <div className="h-5 w-1/2 rounded bg-[#E5EAE7] dark:bg-gray-700" />
        <div className="h-4 w-1/3 rounded bg-[#E5EAE7] dark:bg-gray-700" />
        <div className="h-8 w-24 rounded-full bg-[#E5EAE7] dark:bg-gray-700" />
        <div className="h-10 w-28 rounded-lg bg-[#E5EAE7] dark:bg-gray-700" />
      </div>
    </div>
  );
}