import { useEffect, useState } from 'react';
import { Alert, Box, Button, Chip, CircularProgress, Container, Divider, Paper, Stack, TextField, Typography } from '@mui/material';
import ChangePassword from './ChangePassword';
import useStore from '../state/store';
import { getCurrentUser, updateCurrentUser } from '../network/lib/user';
import { getErrorMessage } from '../lib/errors';

const ROLE_LABELS = { User: 'Użytkownik', Moderator: 'Moderator', Admin: 'Administrator' };

const formatDate = (value) => (value ? new Date(value).toLocaleString('pl-PL') : '–');

function Profile() {
  const setUser = useStore((state) => state.setUser);
  const [user, setProfile] = useState(null);
  const [form, setForm] = useState(null);
  const [message, setMessage] = useState(null);

  useEffect(() => {
    getCurrentUser()
      .then((loaded) => {
        setProfile(loaded);
        setUser(loaded);
      })
      .catch((err) => setMessage({ severity: 'error', text: getErrorMessage(err) }));
  }, [setUser]);

  const handleSave = async () => {
    try {
      const updated = await updateCurrentUser(form);
      setProfile(updated);
      setUser(updated);
      setForm(null);
      setMessage({ severity: 'success', text: 'Dane zostały zapisane.' });
    } catch (err) {
      setMessage({ severity: 'error', text: getErrorMessage(err) });
    }
  };

  if (!user) {
    return (
      <Container maxWidth="sm" sx={{ py: 4 }}>
        {message ? <Alert severity={message.severity}>{message.text}</Alert> : <CircularProgress />}
      </Container>
    );
  }

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Paper sx={{ p: 4 }}>
        <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 2 }}>
          <Typography variant="h5" component="h1">
            {user.firstName} {user.lastName}
          </Typography>
          <Chip label={ROLE_LABELS[user.role] ?? user.role} color="primary" size="small" />
        </Stack>
        {message && <Alert severity={message.severity} sx={{ mb: 2 }}>{message.text}</Alert>}
        {form ? (
          <Box>
            <TextField label="Imię" value={form.firstName} fullWidth margin="dense"
              onChange={(e) => setForm({ ...form, firstName: e.target.value })} />
            <TextField label="Nazwisko" value={form.lastName} fullWidth margin="dense"
              onChange={(e) => setForm({ ...form, lastName: e.target.value })} />
            <TextField label="E-mail" value={form.email} fullWidth margin="dense"
              onChange={(e) => setForm({ ...form, email: e.target.value })} />
            <Stack direction="row" spacing={1} sx={{ mt: 1 }}>
              <Button variant="contained" onClick={handleSave}>Zapisz</Button>
              <Button onClick={() => setForm(null)}>Anuluj</Button>
            </Stack>
          </Box>
        ) : (
          <Box>
            <Typography>E-mail: {user.email}</Typography>
            <Typography color="text.secondary">Konto utworzone: {formatDate(user.createdAt)}</Typography>
            <Typography color="text.secondary">Ostatnie logowanie: {formatDate(user.lastLoginAt)}</Typography>
            <Button sx={{ mt: 2 }} variant="outlined"
              onClick={() => setForm({ firstName: user.firstName, lastName: user.lastName, email: user.email })}>
              Edytuj dane
            </Button>
          </Box>
        )}
        <Divider sx={{ my: 3 }} />
        <ChangePassword />
      </Paper>
    </Container>
  );
}

export default Profile;
