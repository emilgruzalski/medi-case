import { useState } from 'react';
import { Alert, Box, Button, TextField, Typography } from '@mui/material';
import { changePassword } from '../../network/cognito';
import { getValidAccessToken } from '../../lib/auth';
import { getErrorMessage } from '../../lib/errors';
import { validatePassword } from '../../lib/validation';

const EMPTY_FORM = { currentPassword: '', newPassword: '', confirmPassword: '' };

function ChangePassword() {
  const [form, setForm] = useState(EMPTY_FORM);
  const [message, setMessage] = useState(null);

  const handleChange = (event) => setForm({ ...form, [event.target.name]: event.target.value });

  const handleSubmit = async (event) => {
    event.preventDefault();
    const validationError = validatePassword(form.newPassword, form.confirmPassword);
    if (validationError) {
      setMessage({ severity: 'error', text: validationError });
      return;
    }
    try {
      await changePassword(await getValidAccessToken(), form.currentPassword, form.newPassword);
      setForm(EMPTY_FORM);
      setMessage({ severity: 'success', text: 'Hasło zostało zmienione.' });
    } catch (err) {
      setMessage({ severity: 'error', text: getErrorMessage(err) });
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} noValidate>
      <Typography variant="h6" gutterBottom>
        Zmiana hasła
      </Typography>
      {message && <Alert severity={message.severity} sx={{ mb: 2 }}>{message.text}</Alert>}
      <TextField label="Obecne hasło" name="currentPassword" type="password" value={form.currentPassword}
        onChange={handleChange} fullWidth margin="dense" autoComplete="current-password" />
      <TextField label="Nowe hasło" name="newPassword" type="password" value={form.newPassword}
        onChange={handleChange} fullWidth margin="dense" autoComplete="new-password" />
      <TextField label="Powtórz nowe hasło" name="confirmPassword" type="password" value={form.confirmPassword}
        onChange={handleChange} fullWidth margin="dense" autoComplete="new-password" />
      <Button type="submit" variant="outlined" sx={{ mt: 1 }}>
        Zmień hasło
      </Button>
    </Box>
  );
}

export default ChangePassword;
