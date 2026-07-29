import type { Notification } from "../types/notification";
import NotificationItem from "./NotificationItem";

interface NotificationListProps {
  notifications: Notification[];
  onMarkAsRead: (id: string) => void;
}

export default function NotificationList({
  notifications,
  onMarkAsRead,
}: NotificationListProps) {
  return (
    <div className="space-y-4">
      {notifications.map((notification) => (
        <NotificationItem
          key={notification.id}
          notification={notification}
          onMarkAsRead={onMarkAsRead}
        />
      ))}
    </div>
  );
}