import { Link as RouterLink } from 'react-router';
import { Box, Button, Container, Stack, Typography } from '@mui/material';
import Features from './Features';
import HowItWorks from './HowItWorks';
import useStore, { selectIsSignedIn } from '../../state/store';

// The first thing a learner does in the app, so it is also the first thing visitors see.
const EXAMPLE = { question: 'Where does it hurt?', translation: 'Gdzie Pana boli?' };

function Home() {
  const isLoggedIn = useStore(selectIsSignedIn);

  return (
    <>
      <Box sx={{ bgcolor: 'primary.main', color: 'primary.contrastText' }}>
        <Container maxWidth="lg"
          sx={{
            py: { xs: 6, md: 11 }, display: 'grid', gap: { xs: 6, md: 10 }, alignItems: 'center',
            gridTemplateColumns: { xs: 'minmax(0, 1fr)', md: 'minmax(0, 1.1fr) minmax(0, 1fr)' },
          }}>
          <div>
            <Typography variant="h1" sx={{ fontSize: 'clamp(2.4rem, 5vw, 3.9rem)', lineHeight: 1.08, textWrap: 'balance' }}>
              Ucz się rozmawiać z pacjentem w obcym języku
            </Typography>
            <Typography sx={{ mt: 3, mb: 4, maxWidth: '46ch', fontSize: '1.125rem', color: '#d3deea' }}>
              Treści z medycyny ratunkowej, nagrania wymowy, słownik i testy w jednym miejscu.
            </Typography>
            {isLoggedIn ? (
              <Button size="large" color="secondary" variant="contained" component={RouterLink} to="/learning">
                Przejdź do nauki
              </Button>
            ) : (
              <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
                <Button size="large" color="secondary" variant="contained" component={RouterLink} to="/register">
                  Załóż konto
                </Button>
                <Button size="large" color="inherit" variant="outlined" component={RouterLink} to="/login">
                  Zaloguj się
                </Button>
              </Stack>
            )}
          </div>
          <Box component="figure" sx={{ m: 0, p: { xs: 3, md: 4 }, bgcolor: 'primary.dark', borderRadius: 2 }}>
            <Typography lang="en" sx={{ fontSize: 'clamp(2rem, 4vw, 3.1rem)', lineHeight: 1.1, fontWeight: 600, letterSpacing: '-0.02em' }}>
              {EXAMPLE.question}
            </Typography>
            <Typography sx={{ mt: 2, pt: 2, borderTop: 4, borderColor: 'secondary.main', fontSize: '1.35rem' }}>
              {EXAMPLE.translation}
            </Typography>
          </Box>
        </Container>
      </Box>
      <Features />
      <HowItWorks />
    </>
  );
}

export default Home;
