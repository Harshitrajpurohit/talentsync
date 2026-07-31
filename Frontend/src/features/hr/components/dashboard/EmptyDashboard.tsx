export default function EmptyDashboard() {
  return (
    <div className="group rounded-[20px] border-2 border-dashed border-[#E5EAE7] bg-white p-12 text-center transition-all duration-300 hover:border-[#315343] hover:bg-gray-50/50 dark:border-gray-700 dark:bg-gray-900">
      <h2 className="text-xl font-semibold text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
        No dashboard data available
      </h2>

      <p className="mt-2 text-sm text-[#75837D] dark:text-gray-400">
        Recruitment data will appear here once available.
      </p>
    </div>
  );
}