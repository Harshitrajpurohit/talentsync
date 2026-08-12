import type { UserWithRole } from "../../types/user";

import UserActions from "./UserActions";

interface UsersTableProps {
  users: UserWithRole[];

  onOpenDialog: (
    type: "role" | "status" | "delete",
    user: UserWithRole,
  ) => void;
}

export default function UsersTable({
  users,
  onOpenDialog,
}: UsersTableProps) {
  return (
    <div className="overflow-hidden rounded-[20px] border border-[#E5EAE7] bg-white shadow-sm">
      <div className="overflow-x-auto">
        <table className="w-full min-w-[900px]">
          <thead>
            <tr className="border-b border-[#E5EAE7] bg-[#F8FAF9]">
              <th className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D]">
                User
              </th>

              <th className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D]">
                Email
              </th>

              <th className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D]">
                Role
              </th>

              <th className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D]">
                Status
              </th>

              <th className="px-5 py-4 text-left text-xs font-bold uppercase tracking-wider text-[#75837D]">
                Created
              </th>

              <th className="px-5 py-4 text-right text-xs font-bold uppercase tracking-wider text-[#75837D]">
                Actions
              </th>
            </tr>
          </thead>

          <tbody>
            {users.map((user) => (
              <tr
                key={user.id}
                className="border-b border-[#E5EAE7] last:border-b-0 hover:bg-[#F8FAF9]/70"
              >
                <td className="px-5 py-4">
                  <div>
                    <p className="text-sm font-bold text-[#212529]">
                      {user.name}
                    </p>

                    {user.phone && (
                      <p className="mt-0.5 text-xs text-[#75837D]">
                        {user.phone}
                      </p>
                    )}
                  </div>
                </td>

                <td className="px-5 py-4">
                  <p className="text-sm font-medium text-[#212529]">
                    {user.email}
                  </p>
                </td>

                <td className="px-5 py-4">
                  <span className="rounded-full bg-[#EAF7E0] px-3 py-1 text-xs font-bold text-[#315343]">
                    {user.role ?? "Not assigned"}
                  </span>
                </td>

                <td className="px-5 py-4">
                  <span className="text-sm font-semibold text-[#212529]">
                    {user.status}
                  </span>
                </td>

                <td className="px-5 py-4">
                  <span className="text-sm font-medium text-[#75837D]">
                    {new Date(
                      user.createdAt,
                    ).toLocaleDateString()}
                  </span>
                </td>

                <td className="px-5 py-4">
                  <UserActions
                    user={user}
                    onChangeRole={(selectedUser) =>
                      onOpenDialog(
                        "role",
                        selectedUser,
                      )
                    }
                    onChangeStatus={(selectedUser) =>
                      onOpenDialog(
                        "status",
                        selectedUser,
                      )
                    }
                    onDelete={(selectedUser) =>
                      onOpenDialog(
                        "delete",
                        selectedUser,
                      )
                    }
                  />
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}