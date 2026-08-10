import type { UserRole } from "../../../shared/types/role";

export interface UserRoleResponseWithExtra{
    id : string;
    userId : string;
    roleId : string;
    roleName : UserRole;
    userName : string;
    isDeleted : boolean;
    createdAt : string;
}