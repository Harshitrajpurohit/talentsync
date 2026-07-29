import { useState } from "react";

import { notificationApi } from "../api/notificationApi";

export function useMarkAllAsRead() {
  const [loading, setLoading] = useState(false);

  const markAllAsRead = async () => {
    setLoading(true);

    try {
      await notificationApi.markAllAsRead();
    } finally {
      setLoading(false);
    }
  };

  return {
    markAllAsRead,
    loading,
  };
}