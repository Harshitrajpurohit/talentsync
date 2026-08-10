import api from "../../../../shared/api/axios";
import type { UserRole } from "../../../../shared/types/role";

import type { UserRoleResponseWithExtra } from "../../types/userrole";

export async function getUserRolesByThereRoleName(role: UserRole): Promise<Array<UserRoleResponseWithExtra>> {
  const response = await api.get<Array<UserRoleResponseWithExtra>>(
    `/userroles/users/${role}`,
  );

  return response.data;
}