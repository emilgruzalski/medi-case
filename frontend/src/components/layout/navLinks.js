import { canModerate, isAdmin } from '../../lib/roles';

/** Main sections available to the user, in the order they are shown. */
export function navLinks(user) {
  return [
    { to: '/learning', label: 'Nauka' },
    { to: '/glossary', label: 'Słownik' },
    canModerate(user) && { to: '/moderator', label: 'Moderator' },
    isAdmin(user) && { to: '/admin', label: 'Administracja', shortLabel: 'Admin' },
  ].filter(Boolean);
}
