import { Link as RouterLink } from 'react-router-dom';
import { Button, Container, Paper, Stack, Typography } from '@mui/material';
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';

function LoggedOutInfo() {
  return (
    <Container maxWidth="sm" sx={{ py: 6 }}>
      <Paper sx={{ p: 4, textAlign: 'center' }}>
        <LockOutlinedIcon color="primary" sx={{ fontSize: 48 }} />
        <Typography variant="h5" component="h1" gutterBottom>
          Ta część aplikacji wymaga zalogowania
        </Typography>
        <Typography color="text.secondary" sx={{ mb: 3 }}>
          Zaloguj się lub załóż konto, aby korzystać z materiałów do nauki.
        </Typography>
        <Stack direction="row" spacing={2} justifyContent="center">
          <Button variant="contained" component={RouterLink} to="/login">
            Zaloguj się
          </Button>
          <Button variant="outlined" component={RouterLink} to="/register">
            Załóż konto
          </Button>
        </Stack>
      </Paper>
    </Container>
  );
}

export default LoggedOutInfo;
