import { useState } from "react";
import { CheckCheck } from "lucide-react";

import { useNotifications } from "../hooks/useNotifications";
import { useMarkAsRead } from "../hooks/useMarkAsRead";
import { useMarkAllAsRead } from "../hooks/useMarkAllAsRead";
import { useNotificationHub } from "../hooks/useNotificationHub";

import NotificationList from "../components/NotificationList";
import NotificationSkeleton from "../components/NotificationSkeleton";
import EmptyNotifications from "../components/EmptyNotifications";
import NotificationPagination from "../components/NotificationPagination";

export default function NotificationsPage() {
  const [pageNumber, setPageNumber] = useState(1);
  const pageSize = 10;

  const {
    notifications,
    pagination,
    loading,
    refresh,
  } = useNotifications(pageNumber, pageSize);

  const { markAsRead } = useMarkAsRead();

  const {
    markAllAsRead,
    loading: markingAll,
  } = useMarkAllAsRead();

  useNotificationHub({
    onReceive: () => {
      void refresh();
    },
  });

  const handleMarkAsRead = async (id: string) => {
    await markAsRead(id);
    await refresh();
  };

  const handleMarkAllAsRead = async () => {
    await markAllAsRead();
    await refresh();
  };

  if (loading) {
    return (
      <div className="space-y-4">
        <NotificationSkeleton />
        <NotificationSkeleton />
        <NotificationSkeleton />
      </div>
    );
  }

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-4 md:flex-row md:items-center md:justify-between">
        <div>
          <h1 className="text-2xl font-bold text-[#212529] dark:text-white">
            Notifications
          </h1>

          <p className="mt-1 text-sm text-[#75837D] dark:text-gray-400">
            Stay updated with your latest recruitment and account activities.
          </p>
        </div>

        {notifications.length > 0 && (
          <button
            onClick={() => void handleMarkAllAsRead()}
            disabled={markingAll}
            className="group flex items-center justify-center gap-2 rounded-[20px] border border-[#315343] bg-transparent px-4 py-2 text-sm font-semibold text-[#315343] transition-all duration-300 hover:bg-[#315343] hover:text-[#C3F53C] hover:shadow-md disabled:cursor-not-allowed disabled:border-[#E5EAE7] disabled:bg-transparent disabled:text-[#75837D] disabled:shadow-none dark:border-gray-600 dark:text-gray-300"
          >
            <CheckCheck
              size={18}
              className={`transition-colors duration-300 ${
                markingAll ? "animate-pulse" : ""
              }`}
            />
            {markingAll ? "Marking..." : "Mark All as Read"}
          </button>
        )}
      </div>

      {notifications.length === 0 ? (
        <EmptyNotifications />
      ) : (
        <>
          <NotificationList
            notifications={notifications}
            onMarkAsRead={(id) => void handleMarkAsRead(id)}
          />

          {pagination && (
            <NotificationPagination
              pageNumber={pagination.pageNumber}
              pageSize={pagination.pageSize}
              totalRecords={pagination.totalRecords}
              onPageChange={setPageNumber}
            />
          )}
        </>
      )}
    </div>
  );
}