import { useState } from 'react';
import { Alert, Box, Button, TextField, Typography } from '@mui/material';
import { completeNewPassword } from '../../lib/auth';
import { getErrorMessage } from '../../lib/errors';
import { validatePassword } from '../../lib/validation';

function NewPasswordForm({ challenge, onDone }) {
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (event) => {
    event.preventDefault();
    const validationError = validatePassword(password, confirmPassword);
    if (validationError) {
      setError(validationError);
      return;
    }

    setLoading(true);
    try {
      await completeNewPassword(challenge, password);
      onDone();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box component="form" onSubmit={handleSubmit} noValidate>
      <Typography variant="body2" color="text.secondary">
        To Twoje pierwsze logowanie. Ustaw własne hasło zamiast hasła tymczasowego z zaproszenia.
      </Typography>
      {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
      <TextField label="Nowe hasło" type="password" value={password} onChange={(e) => setPassword(e.target.value)}
        fullWidth margin="normal" autoComplete="new-password" autoFocus />
      <TextField label="Powtórz nowe hasło" type="password" value={confirmPassword}
        onChange={(e) => setConfirmPassword(e.target.value)} fullWidth margin="normal" autoComplete="new-password" />
      <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }} disabled={loading}>
        Ustaw hasło i zaloguj się
      </Button>
    </Box>
  );
}

export default NewPasswordForm;
