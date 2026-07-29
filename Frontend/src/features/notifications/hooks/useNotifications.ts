import { useCallback, useEffect, useState } from "react";

import { notificationApi } from "../api/notificationApi";

import type { Notification } from "../types/notification";
import type { PaginationResponse } from "../../../shared/types/pagination";

export function useNotifications(
  pageNumber = 1,
  pageSize = 10,
) {
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [pagination, setPagination] =
    useState<PaginationResponse<Notification> | null>(null);

  const [loading, setLoading] = useState(false);

  const loadNotifications = useCallback(async () => {
    setLoading(true);

    try {
      const response = await notificationApi.getNotifications(
        pageNumber,
        pageSize,
      );

      setNotifications(response.data);
      setPagination(response);
    } finally {
      setLoading(false);
    }
  }, [pageNumber, pageSize]);

  useEffect(() => {
    void loadNotifications();
  }, [loadNotifications]);

  return {
    notifications,
    pagination,
    loading,
    refresh: loadNotifications,
  };
}