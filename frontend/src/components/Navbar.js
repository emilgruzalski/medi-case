import { Link as RouterLink } from 'react-router-dom';
import { AppBar, Box, Button, Toolbar, Typography } from '@mui/material';
import LocalHospitalIcon from '@mui/icons-material/LocalHospital';
import UserBar from './UserBar';
import useStore from '../state/store';

function Navbar() {
  const isLoggedIn = useStore((state) => Boolean(state.token));

  return (
    <AppBar position="static" elevation={1}>
      <Toolbar>
        <LocalHospitalIcon sx={{ mr: 1 }} />
        <Typography variant="h6" component={RouterLink} to="/" sx={{ color: 'inherit', textDecoration: 'none' }}>
          Premedical
        </Typography>
        <Box sx={{ flexGrow: 1 }} />
        {isLoggedIn ? (
          <UserBar />
        ) : (
          <>
            <Button color="inherit" component={RouterLink} to="/login">
              Zaloguj
            </Button>
            <Button color="inherit" component={RouterLink} to="/register">
              Załóż konto
            </Button>
          </>
        )}
      </Toolbar>
    </AppBar>
  );
}

export default Navbar;
