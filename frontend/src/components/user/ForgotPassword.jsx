import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Alert, Box, Button, TextField, Typography } from '@mui/material';
import FormCard from '../common/FormCard';
import { confirmForgotPassword, forgotPassword } from '../../network/cognito';
import { getErrorMessage } from '../../lib/errors';
import { isValidEmail, validatePassword } from '../../lib/validation';

const EMPTY_FORM = { email: '', code: '', password: '', confirmPassword: '' };

function ForgotPassword() {
  const [form, setForm] = useState(EMPTY_FORM);
  const [codeSent, setCodeSent] = useState(false);
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();

  const handleChange = (event) => setForm({ ...form, [event.target.name]: event.target.value });
  const email = form.email.trim().toLowerCase();

  const run = async (action) => {
    setLoading(true);
    try {
      await action();
      setError('');
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  const handleSendCode = (event) => {
    event.preventDefault();
    if (!isValidEmail(email)) {
      setError('Podaj poprawny adres e-mail.');
      return;
    }
    run(async () => {
      await forgotPassword(email);
      setCodeSent(true);
    });
  };

  const handleReset = (event) => {
    event.preventDefault();
    const validationError = validatePassword(form.password, form.confirmPassword);
    if (validationError) {
      setError(validationError);
      return;
    }
    run(async () => {
      await confirmForgotPassword(email, form.code.trim(), form.password);
      navigate('/login', { state: { passwordReset: true } });
    });
  };

  return (
    <FormCard title="Nie pamiętasz hasła?">
      <Typography variant="body2" color="text.secondary">
        {codeSent
          ? 'Wysłaliśmy kod na Twój adres e-mail. Wpisz go razem z nowym hasłem.'
          : 'Podaj adres e-mail konta, a wyślemy na niego kod do ustawienia nowego hasła.'}
      </Typography>
      {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
      {!codeSent ? (
        <Box component="form" onSubmit={handleSendCode} noValidate>
          <TextField label="E-mail" name="email" type="email" value={form.email} onChange={handleChange}
            fullWidth margin="normal" autoComplete="email" autoFocus />
          <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }} disabled={loading}>
            Wyślij kod
          </Button>
        </Box>
      ) : (
        <Box component="form" onSubmit={handleReset} noValidate>
          <TextField label="Kod" name="code" value={form.code} onChange={handleChange}
            fullWidth margin="normal" autoComplete="one-time-code" autoFocus />
          <TextField label="Nowe hasło" name="password" type="password" value={form.password} onChange={handleChange}
            fullWidth margin="normal" autoComplete="new-password" />
          <TextField label="Powtórz nowe hasło" name="confirmPassword" type="password" value={form.confirmPassword}
            onChange={handleChange} fullWidth margin="normal" autoComplete="new-password" />
          <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }} disabled={loading || !form.code.trim()}>
            Ustaw nowe hasło
          </Button>
        </Box>
      )}
    </FormCard>
  );
}

export default ForgotPassword;
