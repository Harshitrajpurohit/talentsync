export default function NotificationSkeleton() {
  return (
    <div className="animate-pulse rounded-[20px] border border-[#E5EAE7] bg-white p-5 shadow-sm dark:border-gray-700 dark:bg-gray-900">
      <div className="mb-3 h-5 w-1/3 rounded bg-[#E5EAE7] dark:bg-gray-700" />
      <div className="mb-2 h-4 w-full rounded bg-[#E5EAE7] dark:bg-gray-700" />
      <div className="mb-4 h-4 w-2/3 rounded bg-[#E5EAE7] dark:bg-gray-700" />
      <div className="h-3 w-24 rounded bg-[#E5EAE7] dark:bg-gray-700" />
    </div>
  );
}