import { Outlet } from 'react-router-dom';
import { Alert, Container } from '@mui/material';
import LoggedOutInfo from '../user/LoggedOutInfo';
import useStore, { selectIsSignedIn } from '../../state/store';

function ProtectedRoute({ allow }) {
  const isSignedIn = useStore(selectIsSignedIn);
  const user = useStore((state) => state.user);

  if (!isSignedIn) {
    return <LoggedOutInfo />;
  }
  if (allow && !allow(user)) {
    return (
      <Container maxWidth="sm" sx={{ py: 6 }}>
        <Alert severity="warning">Nie masz uprawnień do tej części aplikacji.</Alert>
      </Container>
    );
  }
  return <Outlet />;
}

export default ProtectedRoute;
