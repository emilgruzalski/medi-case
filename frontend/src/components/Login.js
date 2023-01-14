import { useState } from 'react';
import { Link as RouterLink, useLocation, useNavigate } from 'react-router-dom';
import { Alert, Box, Button, Link, TextField, Typography } from '@mui/material';
import FormCard from './FormCard';
import { isValidEmail } from '../lib/validation';
import { getErrorMessage } from '../lib/errors';
import { saveSession } from '../lib/session';
import { login } from '../network/lib/session';

function Login() {
  const [form, setForm] = useState({ email: '', password: '' });
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);
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
      saveSession(await login(form));
      navigate('/', { replace: true });
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormCard title="Logowanie">
      <Box component="form" onSubmit={handleSubmit} noValidate>
        {location.state?.registered && !error && (
          <Alert severity="success" sx={{ mb: 2 }}>Konto zostało utworzone. Możesz się zalogować.</Alert>
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
        Nie masz konta? <Link component={RouterLink} to="/register">Zarejestruj się</Link>
      </Typography>
    </FormCard>
  );
}

export default Login;
