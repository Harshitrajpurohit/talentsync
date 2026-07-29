import { useEffect } from "react";
import {
  HubConnection,
  HubConnectionBuilder,
  LogLevel,
} from "@microsoft/signalr";

import { useAuth } from "../../../app/hooks/useAuth";

import type { RealtimeNotification } from "../types/realtimeNotification";

interface UseNotificationHubProps {
  onReceive: (notification: RealtimeNotification) => void;
}


export function useNotificationHub({
  onReceive,
}: UseNotificationHubProps) {
  const { user } = useAuth();

  const token = user?.token;

  useEffect(() => {
    if (!token) return;

    const connection: HubConnection = new HubConnectionBuilder()
      .withUrl(
        `${import.meta.env.VITE_API_URL}/hubs/notifications`,
        {
          accessTokenFactory: () => token,
        },
      )
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    connection.on(
      "ReceiveNotification",
      (notification: RealtimeNotification) => {
        onReceive(notification);
      },
    );

    connection
      .start()
      .catch((error) =>
        console.error("SignalR connection failed:", error),
      );

    return () => {
      void connection.stop();
    };
  }, [token, onReceive]);
}