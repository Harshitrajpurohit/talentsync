import { api } from "../../../shared";

import type { PaginationResponse } from "../../../shared/types/pagination";

import type { Notification } from "../types/notification";

interface UnreadCountResponse {
  unreadCount: number;
}

export const notificationApi = {
  getNotifications: (pageNumber = 1, pageSize = 10) =>
    api
      .get<PaginationResponse<Notification>>("/notifications/my", {
        params: {
          pageNumber,
          pageSize,
        },
      })
      .then((response) => response.data),

  getUnreadCount: () =>
    api
      .get<UnreadCountResponse>("/notifications/unread/count")
      .then((response) => response.data),

  markAsRead: (notificationId: string) =>
    api.put(`/notifications/${notificationId}`),

  markAllAsRead: () =>
    api.put("/notifications/read-all"),
};