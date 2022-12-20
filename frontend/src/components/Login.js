import { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { Alert, Box, Button, Link, TextField, Typography } from '@mui/material';
import FormCard from './FormCard';
import { isValidEmail } from '../lib/validation';

function Login() {
  const [form, setForm] = useState({ email: '', password: '' });
  const [error, setError] = useState('');

  const handleChange = (event) => setForm({ ...form, [event.target.name]: event.target.value });

  const handleSubmit = (event) => {
    event.preventDefault();
    if (!isValidEmail(form.email) || !form.password) {
      setError('Podaj poprawny e-mail i hasło.');
      return;
    }
    setError('');
  };

  return (
    <FormCard title="Logowanie">
      <Box component="form" onSubmit={handleSubmit} noValidate>
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        <TextField label="E-mail" name="email" type="email" value={form.email} onChange={handleChange}
          fullWidth margin="normal" autoComplete="email" autoFocus />
        <TextField label="Hasło" name="password" type="password" value={form.password} onChange={handleChange}
          fullWidth margin="normal" autoComplete="current-password" />
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }}>
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
