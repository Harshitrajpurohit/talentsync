import type { AdminRoleCount } from "../../types/dashboard";

type UsersByRoleProps = {
  roles: AdminRoleCount[];
};

export default function UsersByRole({ roles }: UsersByRoleProps) {
  const total = roles.reduce((sum, role) => sum + role.count, 0);

  return (
    <div className="flex flex-col rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
      {/* Header */}
      <div className="border-b border-[#E5EAE7] p-5 sm:px-8 sm:py-6">
        <h2 className="text-lg font-bold text-[#212529]">
          Users by Role
        </h2>
        <p className="mt-0.5 text-xs font-medium text-[#75837D]">
          Distribution of users across platform roles.
        </p>
      </div>

      {/* Content */}
      <div className="flex-1 p-5 sm:p-8">
        {roles.length === 0 ? (
          <div className="flex h-32 items-center justify-center rounded-xl bg-[#F8FAF9]">
            <p className="text-sm font-medium text-[#75837D]">
              No role data available.
            </p>
          </div>
        ) : (
          <div className="flex flex-col justify-center space-y-6">
            {roles.map((role) => {
              const percentage = total > 0 ? Math.round((role.count / total) * 100) : 0;

              return (
                <div key={role.role}>
                  <div className="mb-2 flex items-center justify-between">
                    <span className="text-sm font-bold text-[#212529]">
                      {role.role}
                    </span>
                    <span className="text-sm font-bold text-[#315343]">
                      {role.count}
                    </span>
                  </div>

                  {/* Progress Track */}
                  <div className="h-2.5 overflow-hidden rounded-full bg-[#EEF3F0]">
                    <div
                      className="h-full rounded-full bg-[#315343] transition-all duration-1000 ease-out"
                      style={{ width: `${percentage}%` }}
                    />
                  </div>

                  <p className="mt-1.5 text-right text-xs font-bold text-[#75837D]">
                    {percentage}%
                  </p>
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
}