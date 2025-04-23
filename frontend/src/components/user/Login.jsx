import { useState } from 'react';
import { Link as RouterLink, useLocation, useNavigate } from 'react-router';
import { Alert, Box, Button, Link, TextField, Typography } from '@mui/material';
import FormCard from '../common/FormCard';
import NewPasswordForm from './NewPasswordForm';
import { isValidEmail } from '../../lib/validation';
import { getSignInErrorMessage } from '../../lib/errors';
import { signIn } from '../../lib/auth';

function Login() {
  const [form, setForm] = useState({ email: '', password: '' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
  const [challenge, setChallenge] = useState(null);
  const navigate = useNavigate();
  const location = useLocation();

  const handleChange = (event) => setForm({ ...form, [event.target.name]: event.target.value });

  const handleSubmit = async (event) => {
    event.preventDefault();
    if (!isValidEmail(form.email) || !form.password) {
      setError('Podaj poprawny e-mail i hasło.');
      return;
    }

    setLoading(true);
    try {
      const newPasswordChallenge = await signIn(form.email, form.password);
      if (newPasswordChallenge) {
        setChallenge(newPasswordChallenge);
        return;
      }
      navigate('/learning', { replace: true });
    } catch (err) {
      if (err.code === 'UserNotConfirmedException') {
        navigate('/confirm', { state: { email: form.email } });
        return;
      }
      setError(getSignInErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  if (challenge) {
    return (
      <FormCard title="Ustaw nowe hasło">
        <NewPasswordForm challenge={challenge} onDone={() => navigate('/learning', { replace: true })} />
      </FormCard>
    );
  }

  return (
    <FormCard title="Logowanie">
      <Box component="form" onSubmit={handleSubmit} noValidate>
        {location.state?.confirmed && !error && (
          <Alert severity="success" sx={{ mb: 2 }}>Konto zostało potwierdzone. Możesz się zalogować.</Alert>
        )}
        {location.state?.passwordReset && !error && (
          <Alert severity="success" sx={{ mb: 2 }}>Hasło zostało zmienione. Zaloguj się nowym hasłem.</Alert>
        )}
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        <TextField label="E-mail" name="email" type="email" value={form.email} onChange={handleChange}
          fullWidth margin="normal" autoComplete="email" autoFocus />
        <TextField label="Hasło" name="password" type="password" value={form.password} onChange={handleChange}
          fullWidth margin="normal" autoComplete="current-password" />
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }} disabled={loading}>
          Zaloguj się
        </Button>
      </Box>
      <Typography variant="body2" sx={{ mt: 2 }}>
        <Link component={RouterLink} to="/forgot-password">Nie pamiętasz hasła?</Link>
      </Typography>
      <Typography variant="body2" sx={{ mt: 1 }}>
        Nie masz konta? <Link component={RouterLink} to="/register">Zarejestruj się</Link>
      </Typography>
    </FormCard>
  );
}

export default Login;
