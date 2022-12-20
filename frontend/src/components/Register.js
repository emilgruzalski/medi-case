import { useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { Alert, Box, Button, Link, TextField, Typography } from '@mui/material';
import FormCard from './FormCard';
import { validateRegistration } from '../lib/validation';

const EMPTY_FORM = { firstName: '', lastName: '', email: '', password: '', confirmPassword: '' };

function Register() {
  const [form, setForm] = useState(EMPTY_FORM);
  const [error, setError] = useState('');

  const handleChange = (event) => setForm({ ...form, [event.target.name]: event.target.value });

  const handleSubmit = (event) => {
    event.preventDefault();
    setError(validateRegistration(form));
  };

  return (
    <FormCard title="Rejestracja">
      <Box component="form" onSubmit={handleSubmit} noValidate>
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        <TextField label="Imię" name="firstName" value={form.firstName} onChange={handleChange}
          fullWidth margin="normal" autoComplete="given-name" autoFocus />
        <TextField label="Nazwisko" name="lastName" value={form.lastName} onChange={handleChange}
          fullWidth margin="normal" autoComplete="family-name" />
        <TextField label="E-mail" name="email" type="email" value={form.email} onChange={handleChange}
          fullWidth margin="normal" autoComplete="email" />
        <TextField label="Hasło" name="password" type="password" value={form.password} onChange={handleChange}
          fullWidth margin="normal" autoComplete="new-password" />
        <TextField label="Powtórz hasło" name="confirmPassword" type="password" value={form.confirmPassword}
          onChange={handleChange} fullWidth margin="normal" autoComplete="new-password" />
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }}>
          Załóż konto
        </Button>
      </Box>
      <Typography variant="body2" sx={{ mt: 2 }}>
        Masz już konto? <Link component={RouterLink} to="/login">Zaloguj się</Link>
      </Typography>
    </FormCard>
  );
}

export default Register;
