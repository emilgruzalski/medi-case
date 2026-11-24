import { User } from '../network/types';

export const Roles = {
  User: 'User',
  Moderator: 'Moderator',
  Admin: 'Admin',
} as const;

export const canModerate = (user: User | null) => user?.role === Roles.Moderator || user?.role === Roles.Admin;

export const isAdmin = (user: User | null) => user?.role === Roles.Admin;
