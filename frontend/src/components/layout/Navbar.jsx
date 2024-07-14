import { Link as RouterLink, NavLink } from 'react-router-dom';
import { AppBar, Box, Button, IconButton, Toolbar, Tooltip, Typography, useMediaQuery } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import TranslateIcon from '@mui/icons-material/Translate';
import UserBar from './UserBar';
import NavMenu from './NavMenu';
import Logo from '../common/Logo';
import useStore, { selectIsSignedIn } from '../../state/store';
import { canModerate, isAdmin } from '../../lib/roles';

const ACTIVE_LINK_STYLE = { '&.active': { bgcolor: 'rgba(255, 255, 255, 0.15)' } };

function NavButton({ to, children }) {
  return (
    <Button color="inherit" component={NavLink} to={to} sx={ACTIVE_LINK_STYLE}>
      {children}
    </Button>
  );
}

function Navbar() {
  const user = useStore((state) => state.user);
  const isLoggedIn = useStore(selectIsSignedIn);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  const links = [
    { to: '/learning', label: 'Nauka' },
    canModerate(user) && { to: '/moderator', label: 'Moderator' },
    isAdmin(user) && { to: '/admin', label: 'Administracja' },
  ].filter(Boolean);

  return (
    <AppBar position="sticky" elevation={1}>
      <Toolbar>
        <Box sx={{ mr: 1 }}>
          <Logo size={32} />
        </Box>
        <Typography variant="h6" component={RouterLink} to="/"
          sx={{ color: 'inherit', textDecoration: 'none', mr: 3, display: { xs: 'none', sm: 'block' } }}>
          MediCase
        </Typography>
        {isLoggedIn && isMobile && <NavMenu links={links} />}
        {isLoggedIn && !isMobile && links.map((link) => <NavButton key={link.to} to={link.to}>{link.label}</NavButton>)}
        <Box sx={{ flexGrow: 1 }} />
        {isLoggedIn ? (
          <>
            <Tooltip title="Zmień język">
              <IconButton color="inherit" component={RouterLink} to="/language">
                <TranslateIcon />
              </IconButton>
            </Tooltip>
            <UserBar />
          </>
        ) : (
          <>
            <NavButton to="/login">Zaloguj</NavButton>
            <NavButton to="/register">Załóż konto</NavButton>
          </>
        )}
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
