import { Bell, CheckCircle2 } from "lucide-react";
import type { Notification } from "../types/notification";

interface NotificationItemProps {
  notification: Notification;
  onMarkAsRead: (id: string) => void;
}

export default function NotificationItem({
  notification,
  onMarkAsRead,
}: NotificationItemProps) {
  return (
    <div
      className={`group rounded-[20px] border p-4 transition-all duration-300 hover:shadow-md ${
        notification.isRead
          ? "border-[#E5EAE7] bg-white hover:border-[#315343] dark:border-gray-700 dark:bg-gray-900"
          : "border-[#315343]/40 bg-[#E5EAE7]/30 hover:border-[#315343] dark:border-[#315343] dark:bg-[#315343]/10"
      }`}
    >
      <div className="flex items-start justify-between gap-4">
        <div className="flex gap-3">
          <Bell
            size={20}
            className={`mt-1 transition-colors duration-300 group-hover:text-[#C3F53C] ${
              notification.isRead ? "text-[#75837D]" : "text-[#315343]"
            }`}
          />

          <div>
            <h3 className="font-semibold text-[#212529] transition-colors duration-300 group-hover:text-[#315343] dark:text-white">
              {notification.title}
            </h3>

            <p className="mt-1 text-sm text-[#75837D] dark:text-gray-300">
              {notification.message}
            </p>

            <p className="mt-2 text-xs text-[#75837D]/80">
              {new Date(notification.createdAt).toLocaleString()}
            </p>
          </div>
        </div>

        {!notification.isRead && (
          <button
            onClick={() => onMarkAsRead(notification.id)}
            className="flex items-center gap-1 rounded-[20px] border border-[#315343] bg-transparent px-3 py-1 text-sm font-medium text-[#315343] transition-all duration-300 hover:bg-[#315343] hover:text-[#C3F53C]"
          >
            <CheckCircle2 size={16} />
            Read
          </button>
        )}
      </div>
    </div>
  );
}