import { BellOff } from "lucide-react";

export default function EmptyNotifications() {
  return (
    <div className="group rounded-[20px] border-2 border-dashed border-[#E5EAE7] bg-white px-6 py-12 text-center transition-all duration-300 hover:border-[#315343] hover:bg-gray-50/50 dark:border-gray-700 dark:bg-gray-900">
      <BellOff
        size={48}
        className="mx-auto text-[#75837D] transition-colors duration-300 group-hover:text-[#C3F53C]"
      />

      <h3 className="mt-4 text-lg font-semibold text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
        No notifications
      </h3>

      <p className="mt-2 text-sm text-[#75837D] dark:text-gray-400">
        You're all caught up. New notifications will appear here.
      </p>
    </div>
  );
}