import { Box, Container, Typography } from '@mui/material';

const FEATURES = [
  { title: 'Rozmowa z pacjentem', text: 'Gotowe pytania, które pomagają zebrać wywiad w języku pacjenta.' },
  { title: 'Nagrania wymowy', text: 'Każde pytanie i karta mogą mieć nagranie, aby ćwiczyć poprawną wymowę.' },
  { title: 'Słownik', text: 'Najważniejsze pojęcia z medycyny ratunkowej z krótkimi definicjami.' },
  { title: 'Testy', text: 'Sprawdź wiedzę – odpowiedzi są oceniane od razu, a test możesz powtórzyć.' },
];

function Features() {
  return (
    <Container maxWidth="lg" sx={{ py: { xs: 6, md: 10 } }}>
      <Box component="dl" sx={{ m: 0, display: 'grid', columnGap: 8, gridTemplateColumns: { xs: '1fr', md: '1fr 1fr' } }}>
        {FEATURES.map((feature) => (
          <Box key={feature.title} sx={{ py: 3, borderTop: 1, borderColor: 'divider' }}>
            <Typography component="dt" variant="h5">{feature.title}</Typography>
            <Typography component="dd" color="text.secondary" sx={{ m: 0, mt: 0.5, maxWidth: '48ch' }}>{feature.text}</Typography>
          </Box>
        ))}
      </Box>
    </Container>
  );
}

export default Features;
