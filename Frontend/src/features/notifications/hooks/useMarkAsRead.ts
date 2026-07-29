import { useState } from "react";

import { notificationApi } from "../api/notificationApi";

export function useMarkAsRead() {
  const [loading, setLoading] = useState(false);

  const markAsRead = async (notificationId: string) => {
    setLoading(true);

    try {
      await notificationApi.markAsRead(notificationId);
    } finally {
      setLoading(false);
    }
  };

  return {
    markAsRead,
    loading,
  };
}