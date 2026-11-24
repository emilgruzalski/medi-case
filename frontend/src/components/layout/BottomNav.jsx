import { NavLink } from 'react-router';
import { Box } from '@mui/material';
import useStore from '../../state/store';
import { navLinks } from './navLinks';

export const BOTTOM_NAV_HEIGHT = 64;

function BottomNav() {
  const user = useStore((state) => state.user);
  const links = [...navLinks(user), { to: '/profile', label: 'Konto' }];

  return (
    <Box component="nav" aria-label="Główna nawigacja"
      sx={{
        position: 'fixed', zIndex: 'appBar', right: 0, bottom: 0, left: 0, display: 'flex',
        pb: 'env(safe-area-inset-bottom)', bgcolor: 'background.paper', borderTop: 1, borderColor: 'divider',
      }}>
      {links.map((link) => (
        <Box key={link.to} component={NavLink} to={link.to}
          sx={{
            flex: 1, minWidth: 0, height: BOTTOM_NAV_HEIGHT, display: 'flex', alignItems: 'center', justifyContent: 'center',
            color: 'text.primary', fontSize: links.length > 4 ? 13 : 15, textDecoration: 'none',
            '&.active': { color: 'primary.main', fontWeight: 600, boxShadow: (theme) => `inset 0 4px 0 ${theme.palette.secondary.main}` },
          }}>
          {link.shortLabel ?? link.label}
        </Box>
      ))}
    </Box>
  );
}

export default BottomNav;
