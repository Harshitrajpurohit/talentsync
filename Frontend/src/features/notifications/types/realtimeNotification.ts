import type { NotificationCategory } from "../../../shared/types/notification";

export interface RealtimeNotification {
  id: string;
  userId: string;
  title: string;
  message: string;
  category: NotificationCategory;
  isRead: boolean;
  readAt: string | null;
  createdAt: string;
}