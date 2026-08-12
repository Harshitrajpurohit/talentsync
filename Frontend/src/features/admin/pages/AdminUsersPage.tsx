import { useState } from "react";

import Pagination from "../../../shared/components/Pagination";
import LoadingSpinner from "../../../shared/components/LoadingSpinner";
import { useDebounce } from "../../../shared/hooks/useDebounce";

import type { UserRole } from "../../../shared/types/role";
import type { UserStatus } from "../../../shared/types/user";

import {
  UsersFilters,
  UsersTable,
  UsersEmptyState,
} from "../components/users";

import {
  ChangeRoleDialog,
  ChangeStatusDialog,
  DeleteUserDialog,
} from "../components/users/dialogs";

import type { UserWithRole } from "../types/user";

import { useUsers } from "../hooks/users/useUsers";

type DialogType = "role" | "status" | "delete" | null;

export default function AdminUsersPage() {
  const [pageNumber, setPageNumber] = useState(1);
  const [pageSize] = useState(10);

  const [search, setSearch] = useState("");
  const [role, setRole] = useState<UserRole | undefined>();
  const [status, setStatus] = useState<UserStatus | undefined>();

  const [selectedUser, setSelectedUser] =
    useState<UserWithRole | null>(null);

  const [activeDialog, setActiveDialog] =
    useState<DialogType>(null);

  const debouncedSearch = useDebounce(search, 500);

  const {
    users: usersResponse,
    loading,
    error,
    refetch,
  } = useUsers({
    pageNumber,
    pageSize,
    search: debouncedSearch.trim() || undefined,
    role,
    status,
  });

  const users = usersResponse?.data ?? [];
  const totalRecords = usersResponse?.totalRecords ?? 0;

  const handleSearchChange = (value: string) => {
    setSearch(value);
    setPageNumber(1);
  };

  const handleRoleChange = (value: UserRole | undefined) => {
    setRole(value);
    setPageNumber(1);
  };

  const handleStatusChange = (
    value: UserStatus | undefined,
  ) => {
    setStatus(value);
    setPageNumber(1);
  };

  const handleOpenDialog = (
    type: Exclude<DialogType, null>,
    user: UserWithRole,
  ) => {
    setSelectedUser(user);
    setActiveDialog(type);
  };

  const handleCloseDialog = () => {
    setActiveDialog(null);
    setSelectedUser(null);
  };

  const handleDialogSuccess = async () => {
    await refetch();
  };

  const hasFilters =
    search.trim().length > 0 ||
    role !== undefined ||
    status !== undefined;

  return (
    <div className="space-y-6 animate-in fade-in slide-in-from-bottom-4 duration-500 fill-mode-both">
      {/* Page Header */}
      <div>
        <h1 className="text-2xl font-bold tracking-tight text-[#212529]">
          Users
        </h1>

        <p className="mt-1 max-w-2xl text-sm font-medium text-[#75837D]">
          Manage user accounts, roles, and account status
          across TalentSync.
        </p>
      </div>

      {/* Filters */}
      <UsersFilters
        search={search}
        role={role}
        status={status}
        onSearchChange={handleSearchChange}
        onRoleChange={handleRoleChange}
        onStatusChange={handleStatusChange}
      />

      {/* Error */}
      {error && !loading && (
        <div className="rounded-xl border border-red-200 bg-red-50 px-5 py-4">
          <p className="text-sm font-bold text-red-700">
            {error}
          </p>
        </div>
      )}

      {/* Content */}
      {loading ? (
        <section className="flex min-h-[320px] items-center justify-center rounded-2xl border border-[#E5EAE7] bg-white shadow-sm">
          <LoadingSpinner />
        </section>
      ) : users.length === 0 ? (
        <UsersEmptyState hasFilters={hasFilters} />
      ) : (
        <div className="space-y-4">
          {/* Results Header */}
          <div className="flex flex-col gap-2 px-1 sm:flex-row sm:items-end sm:justify-between">
            <div>
              <h2 className="text-lg font-bold text-[#212529]">
                All Users
              </h2>

              <p className="mt-0.5 text-xs font-bold uppercase tracking-wider text-[#75837D]">
                {totalRecords}{" "}
                {totalRecords === 1 ? "user" : "users"} found
              </p>
            </div>

            <p className="text-xs font-bold uppercase tracking-wider text-[#75837D]">
              Page{" "}
              <span className="text-[#212529]">
                {pageNumber}
              </span>
            </p>
          </div>

          {/* Users Table */}
          <UsersTable
            users={users}
            onOpenDialog={handleOpenDialog}
          />

          {/* Pagination */}
          <Pagination
            pageNumber={pageNumber}
            pageSize={pageSize}
            totalRecords={totalRecords}
            onPageChange={setPageNumber}
          />
        </div>
      )}

      {/* Dialogs */}
      {activeDialog === "role" && selectedUser && (
        <ChangeRoleDialog
          user={selectedUser}
          onClose={handleCloseDialog}
          onSuccess={handleDialogSuccess}
        />
      )}

      {activeDialog === "status" && selectedUser && (
        <ChangeStatusDialog
          user={selectedUser}
          onClose={handleCloseDialog}
          onSuccess={handleDialogSuccess}
        />
      )}

      {activeDialog === "delete" && selectedUser && (
        <DeleteUserDialog
          user={selectedUser}
          onClose={handleCloseDialog}
          onSuccess={handleDialogSuccess}
        />
      )}
    </div>
  );
}