import { Role } from './role';

export interface User {
    id: string;
    userName: string;
    firstName: string;
    lastName: string;
    email: string;
    phone: string;
    avatar: string;
    gender: string;
    isActive: boolean;
    isHidden: boolean;
    isDeleted: boolean;
    roles: Role[];
}
