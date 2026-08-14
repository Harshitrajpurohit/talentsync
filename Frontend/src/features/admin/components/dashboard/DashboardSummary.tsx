import {
  Users,
  UserCheck,
  UserX,
  UserRoundX,
} from "lucide-react";

import { StatCard } from "../../../../shared/components/StatCard";

type DashboardSummaryProps = {
  totalUsers: number;
  activeUsers: number;
  inactiveUsers: number;
  suspendedUsers: number;
};

export default function DashboardSummary({
  totalUsers,
  activeUsers,
  inactiveUsers,
  suspendedUsers,
}: DashboardSummaryProps) {
  return (
    <div className="grid grid-cols-1 gap-4 sm:grid-cols-2 xl:grid-cols-4">
      <StatCard
        title="Total Users"
        value={totalUsers}
        icon={Users}
      />

      <StatCard
        title="Active Users"
        value={activeUsers}
        icon={UserCheck}
      />

      <StatCard
        title="Inactive Users"
        value={inactiveUsers}
        icon={UserX}
      />

      <StatCard
        title="Suspended Users"
        value={suspendedUsers}
        icon={UserRoundX}
      />
    </div>
  );
}