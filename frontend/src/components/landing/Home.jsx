import { Link as RouterLink } from 'react-router';
import { Box, Button, Container, Stack, Typography } from '@mui/material';
import Logo from '../common/Logo';
import Features from './Features';
import HowItWorks from './HowItWorks';
import useStore, { selectIsSignedIn } from '../../state/store';

const LOGO_SIZE = 120;

function Home() {
  const isLoggedIn = useStore(selectIsSignedIn);

  return (
    <>
      <Box sx={{ background: 'linear-gradient(180deg, #ffffff 0%, #f5f6f8 100%)' }}>
        <Container maxWidth="md" sx={{ py: 10, textAlign: 'center' }}>
          <Stack alignItems="center" spacing={3}>
            <Logo size={LOGO_SIZE} />
            <Typography variant="h2" component="h1" fontWeight="bold">
              MediCase
            </Typography>
            <Typography variant="h6" color="text.secondary" sx={{ maxWidth: 560 }}>
              Ucz się rozmawiać z pacjentem w obcym języku. Treści z medycyny ratunkowej, nagrania wymowy, słownik i testy
              w jednym miejscu.
            </Typography>
            {isLoggedIn ? (
              <Button size="large" variant="contained" component={RouterLink} to="/learning">
                Przejdź do nauki
              </Button>
            ) : (
              <Stack direction="row" spacing={2}>
                <Button size="large" variant="contained" component={RouterLink} to="/register">
                  Załóż konto
                </Button>
                <Button size="large" variant="outlined" component={RouterLink} to="/login">
                  Zaloguj się
                </Button>
              </Stack>
            )}
          </Stack>
        </Container>
      </Box>
      <Features />
      <HowItWorks />
    </>
  );
}

export default Home;
