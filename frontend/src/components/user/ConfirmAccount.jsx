import { useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import { Alert, Box, Button, TextField, Typography } from '@mui/material';
import FormCard from '../common/FormCard';
import { confirmSignUp } from '../../network/cognito';
import { getErrorMessage } from '../../lib/errors';

function ConfirmAccount() {
  const location = useLocation();
  const navigate = useNavigate();
  const [email, setEmail] = useState(location.state?.email ?? '');
  const [code, setCode] = useState('');
  const [error, setError] = useState('');
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (event) => {
    event.preventDefault();
    setLoading(true);
    try {
      await confirmSignUp(email.trim().toLowerCase(), code.trim());
      navigate('/login', { state: { confirmed: true } });
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <FormCard title="Potwierdź konto">
      <Typography variant="body2" color="text.secondary">
        Wysłaliśmy kod weryfikacyjny na Twój adres e-mail. Wpisz go poniżej, aby aktywować konto.
      </Typography>
      <Box component="form" onSubmit={handleSubmit} noValidate>
        {error && <Alert severity="error" sx={{ mt: 2 }}>{error}</Alert>}
        <TextField label="E-mail" type="email" value={email} onChange={(e) => setEmail(e.target.value)}
          fullWidth margin="normal" autoComplete="email" />
        <TextField label="Kod" value={code} onChange={(e) => setCode(e.target.value)}
          fullWidth margin="normal" autoComplete="one-time-code" autoFocus />
        <Button type="submit" variant="contained" fullWidth sx={{ mt: 2 }} disabled={loading || !code.trim()}>
          Potwierdź
        </Button>
      </Box>
    </FormCard>
  );
}

export default ConfirmAccount;
