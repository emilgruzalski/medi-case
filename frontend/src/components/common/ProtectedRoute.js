import { Outlet } from 'react-router-dom';
import { Alert, Container } from '@mui/material';
import LoggedOutInfo from '../user/LoggedOutInfo';
import useStore from '../../state/store';

function ProtectedRoute({ allow }) {
  const token = useStore((state) => state.token);
  const user = useStore((state) => state.user);

  if (!token) {
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
