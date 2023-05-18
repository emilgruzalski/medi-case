import { Card, CardContent, Container, Grid, Typography } from '@mui/material';
import ForumOutlinedIcon from '@mui/icons-material/ForumOutlined';
import MenuBookOutlinedIcon from '@mui/icons-material/MenuBookOutlined';
import QuizOutlinedIcon from '@mui/icons-material/QuizOutlined';
import VolumeUpOutlinedIcon from '@mui/icons-material/VolumeUpOutlined';

const FEATURES = [
  {
    icon: <ForumOutlinedIcon color="primary" fontSize="large" />,
    title: 'Rozmowa z pacjentem',
    text: 'Gotowe pytania, które pomagają zebrać wywiad w języku pacjenta.',
  },
  {
    icon: <VolumeUpOutlinedIcon color="primary" fontSize="large" />,
    title: 'Nagrania wymowy',
    text: 'Każde pytanie i karta mogą mieć nagranie, aby ćwiczyć poprawną wymowę.',
  },
  {
    icon: <MenuBookOutlinedIcon color="primary" fontSize="large" />,
    title: 'Słownik',
    text: 'Najważniejsze pojęcia z medycyny ratunkowej z krótkimi definicjami.',
  },
  {
    icon: <QuizOutlinedIcon color="primary" fontSize="large" />,
    title: 'Testy',
    text: 'Sprawdź wiedzę – odpowiedzi są oceniane od razu, a test możesz powtórzyć.',
  },
];

function Features() {
  return (
    <Container maxWidth="lg" sx={{ py: 8 }}>
      <Grid container spacing={3}>
        {FEATURES.map((feature) => (
          <Grid item xs={12} sm={6} md={3} key={feature.title}>
            <Card sx={{ height: '100%' }}>
              <CardContent>
                {feature.icon}
                <Typography variant="h6" sx={{ mt: 1 }}>
                  {feature.title}
                </Typography>
                <Typography color="text.secondary">{feature.text}</Typography>
              </CardContent>
            </Card>
          </Grid>
        ))}
      </Grid>
    </Container>
  );
}

export default Features;
