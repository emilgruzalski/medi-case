import { Link as RouterLink, NavLink } from 'react-router';
import { AppBar, Box, Button, Toolbar, Typography, useMediaQuery } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import TranslateIcon from '@mui/icons-material/Translate';
import UserBar from './UserBar';
import Logo from '../common/Logo';
import useStore, { selectIsSignedIn } from '../../state/store';
import { navLinks } from './navLinks';

const LINK_STYLE = {
  display: 'flex', alignItems: 'center', px: 2, color: '#d3deea', fontWeight: 500, textDecoration: 'none',
  '&:hover': { color: '#ffffff' },
  '&.active': { color: '#ffffff', boxShadow: (theme) => `inset 0 -4px 0 ${theme.palette.secondary.main}` },
};

function Navbar() {
  const user = useStore((state) => state.user);
  const isLoggedIn = useStore(selectIsSignedIn);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  return (
    <AppBar position="sticky">
      <Toolbar sx={{ gap: 1 }}>
        <Box component={RouterLink} to="/"
          sx={{ display: 'flex', alignItems: 'center', gap: 1.5, mr: 2, color: 'inherit', textDecoration: 'none' }}>
          <Logo size={34} />
          <Typography variant="h6" component="span">MediCase</Typography>
        </Box>
        {isLoggedIn && !isMobile && (
          <Box component="nav" aria-label="Główna nawigacja" sx={{ display: 'flex', alignSelf: 'stretch' }}>
            {navLinks(user).map((link) => (
              <Box key={link.to} component={NavLink} to={link.to} sx={LINK_STYLE}>{link.label}</Box>
            ))}
          </Box>
        )}
        <Box sx={{ flexGrow: 1 }} />
        {isLoggedIn ? (
          <>
            <Button color="inherit" component={RouterLink} to="/language" startIcon={<TranslateIcon />}>
              Języki
            </Button>
            {!isMobile && <UserBar />}
          </>
        ) : (
          <>
            <Button color="inherit" component={RouterLink} to="/login">Zaloguj</Button>
            <Button color="secondary" variant="contained" component={RouterLink} to="/register"
              sx={{ display: { xs: 'none', sm: 'inline-flex' } }}>
              Załóż konto
            </Button>
          </>
        )}
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
