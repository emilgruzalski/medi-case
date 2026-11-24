import { Outlet } from 'react-router';
import { Box, useMediaQuery } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import Navbar from './Navbar';
import BottomNav, { BOTTOM_NAV_HEIGHT } from './BottomNav';
import Footer from './Footer';
import useStore, { selectIsSignedIn } from '../../state/store';

function Layout() {
  const isSignedIn = useStore(selectIsSignedIn);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const hasBottomNav = isMobile && isSignedIn;

  return (
    <div className="app">
      <Navbar />
      <Box component="main" className="app-content"
        sx={{ pb: hasBottomNav ? `calc(${BOTTOM_NAV_HEIGHT}px + env(safe-area-inset-bottom))` : 0 }}>
        <Outlet />
      </Box>
      {hasBottomNav ? <BottomNav /> : <Footer />}
    </div>
  );
}

export default Layout;
