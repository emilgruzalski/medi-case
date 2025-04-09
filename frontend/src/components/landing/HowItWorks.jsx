import { Avatar, Box, Container, Grid, Typography } from '@mui/material';

const STEPS = [
  { title: 'Wybierz języki', text: 'Język, którego się uczysz, i swój język, w którym zobaczysz tłumaczenia.' },
  { title: 'Ucz się', text: 'Przechodź przez działy, czytaj karty, słuchaj nagrań i powtarzaj pytania.' },
  { title: 'Sprawdź się', text: 'Na końcu działu rozwiąż test i zobacz swój wynik.' },
];

function HowItWorks() {
  return (
    <Box sx={{ bgcolor: 'background.paper', py: 8 }}>
      <Container maxWidth="lg">
        <Typography variant="h4" component="h2" align="center" gutterBottom>
          Jak to działa?
        </Typography>
        <Grid container spacing={4} sx={{ mt: 2 }}>
          {STEPS.map((step, index) => (
            <Grid size={{ xs: 12, md: 4 }} key={step.title} sx={{ textAlign: 'center' }}>
              <Avatar sx={{ bgcolor: 'secondary.main', mx: 'auto', mb: 2 }}>{index + 1}</Avatar>
              <Typography variant="h6">{step.title}</Typography>
              <Typography color="text.secondary">{step.text}</Typography>
            </Grid>
          ))}
        </Grid>
      </Container>
    </Box>
  );
}

export default HowItWorks;
