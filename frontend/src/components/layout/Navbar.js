import { Link as RouterLink, NavLink } from 'react-router-dom';
import { AppBar, Box, Button, IconButton, Toolbar, Tooltip, Typography } from '@mui/material';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import TranslateIcon from '@mui/icons-material/Translate';
import UserBar from './UserBar';
import useStore from '../../state/store';
import { canModerate } from '../../lib/roles';

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
  const isLoggedIn = useStore((state) => Boolean(state.token));

  return (
    <AppBar position="sticky" elevation={1}>
      <Toolbar>
        <LocalHospitalIcon sx={{ mr: 1 }} />
        <Typography variant="h6" component={RouterLink} to="/" sx={{ color: 'inherit', textDecoration: 'none', mr: 3 }}>
          Premedical
        </Typography>
        {isLoggedIn && (
          <>
            <NavButton to="/learning">Nauka</NavButton>
            {canModerate(user) && <NavButton to="/moderator">Moderator</NavButton>}
          </>
        )}
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
