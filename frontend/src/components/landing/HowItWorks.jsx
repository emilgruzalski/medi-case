import { Box, Container, Typography } from '@mui/material';

const STEPS = [
  { title: 'Wybierz języki', text: 'Język, którego się uczysz, i swój język, w którym zobaczysz tłumaczenia.' },
  { title: 'Ucz się', text: 'Przechodź przez działy, czytaj karty, słuchaj nagrań i powtarzaj pytania.' },
  { title: 'Sprawdź się', text: 'Na końcu działu rozwiąż test i zobacz swój wynik.' },
];

function HowItWorks() {
  return (
    <Box sx={{ bgcolor: 'background.paper', py: { xs: 6, md: 10 } }}>
      <Container maxWidth="lg">
        <Typography variant="h4" component="h2">Jak to działa?</Typography>
        <Box component="ol" sx={{ m: 0, mt: 4, p: 0, listStyle: 'none', display: 'grid', gap: 4, gridTemplateColumns: { xs: '1fr', md: 'repeat(3, 1fr)' } }}>
          {STEPS.map((step, index) => (
            <Box component="li" key={step.title}>
              <Box aria-hidden="true"
                sx={{ width: 48, height: 48, mb: 2, display: 'flex', alignItems: 'center', justifyContent: 'center', bgcolor: 'secondary.main', borderRadius: 2, fontSize: '1.25rem', fontWeight: 600 }}>
                {index + 1}
              </Box>
              <Typography variant="h6" component="h3">{step.title}</Typography>
              <Typography color="text.secondary">{step.text}</Typography>
            </Box>
          ))}
        </Box>
      </Container>
    </Box>
  );
}

export default HowItWorks;
