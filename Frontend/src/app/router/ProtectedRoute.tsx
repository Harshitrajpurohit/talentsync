import { useEffect, useSyncExternalStore } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { jwtDecode } from "jwt-decode";

import { useAuth } from "../hooks/useAuth";
import LoadingSpinner from "../../shared/components/LoadingSpinner";
import { clearAuth, getToken } from "../../shared";

interface Props {
  allowedRoles: string[];
}

interface JwtPayload {
  exp: number;
  role?: string;
}

// 1. External store subscription helper for time
function subscribeToClock(callback: () => void) {
  // Checks token expiration status periodically or on mount
  const interval = setInterval(callback, 10000); // optional periodic check
  return () => clearInterval(interval);
}

function getSnapshotTime() {
  return Math.floor(Date.now() / 1000);
}

function getServerSnapshotTime() {
  return 0;
}

// 2. Custom hook to read current timestamp pure-friendly
function useCurrentTime() {
  return useSyncExternalStore(
    subscribeToClock,
    getSnapshotTime,
    getServerSnapshotTime
  );
}

export default function ProtectedRoute({ allowedRoles }: Props) {
  const { user, isAuthenticated, isLoading } = useAuth();
  const token = getToken();
  const currentTime = useCurrentTime();

  // 3. Helper to parse expiration relative to subscriber time
  let isExpired = true;
  if (token) {
    try {
      const decoded = jwtDecode<JwtPayload>(token);
      isExpired = decoded.exp < currentTime;
    } catch {
      isExpired = true;
    }
  }

  // 4. Side effect (clearing auth) handled safely
  useEffect(() => {
    if (token && isExpired) {
      clearAuth();
    }
  }, [token, isExpired]);

  // 5. Loading State
  if (isLoading) {
    return <LoadingSpinner />;
  }

  // 6. Token/Expiration Validation
  if (!token || isExpired) {
    return <Navigate to="/login" replace />;
  }

  // 7. Auth Context Validation
  if (!isAuthenticated || !user) {
    return <Navigate to="/login" replace />;
  }

  // 8. Role Authorization
  if (!allowedRoles.includes(user.role)) {
    return <Navigate to="/unauthorized" replace />;
  }

  return <Outlet />;
}