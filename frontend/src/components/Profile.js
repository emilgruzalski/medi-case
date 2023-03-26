import { useEffect, useState } from 'react';
import { Alert, Chip, CircularProgress, Container, Paper, Stack, Typography } from '@mui/material';
import useStore from '../state/store';
import { getCurrentUser } from '../network/lib/user';
import { getErrorMessage } from '../lib/errors';

const ROLE_LABELS = { User: 'Użytkownik', Moderator: 'Moderator', Admin: 'Administrator' };

const formatDate = (value) => (value ? new Date(value).toLocaleString('pl-PL') : '–');

function Profile() {
  const setUser = useStore((state) => state.setUser);
  const [user, setProfile] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    getCurrentUser()
      .then((loaded) => {
        setProfile(loaded);
        setUser(loaded);
      })
      .catch((err) => setError(getErrorMessage(err)));
  }, [setUser]);

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      {error && <Alert severity="error">{error}</Alert>}
      {!user && !error && <CircularProgress />}
      {user && (
        <Paper sx={{ p: 4 }}>
          <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 2 }}>
            <Typography variant="h5" component="h1">
              {user.firstName} {user.lastName}
            </Typography>
            <Chip label={ROLE_LABELS[user.role] ?? user.role} color="primary" size="small" />
          </Stack>
          <Typography>E-mail: {user.email}</Typography>
          <Typography color="text.secondary">Konto utworzone: {formatDate(user.createdAt)}</Typography>
          <Typography color="text.secondary">Ostatnie logowanie: {formatDate(user.lastLoginAt)}</Typography>
        </Paper>
      )}
    </Container>
  );
}

export default Profile;
