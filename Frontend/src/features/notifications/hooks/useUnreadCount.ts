import { useCallback, useEffect, useState } from "react";

import { notificationApi } from "../api/notificationApi";

export function useUnreadCount() {
  const [unreadCount, setUnreadCount] = useState(0);

  const [loading, setLoading] = useState(false);

  const loadUnreadCount = useCallback(async () => {
    setLoading(true);

    try {
      const response = await notificationApi.getUnreadCount();

      setUnreadCount(response.unreadCount);
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadUnreadCount();
  }, [loadUnreadCount]);

  return {
    unreadCount,
    loading,
    refresh: loadUnreadCount,
    setUnreadCount,
  };
}