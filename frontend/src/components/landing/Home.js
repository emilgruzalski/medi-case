import { Container, Typography } from '@mui/material';

function Home() {
  return (
    <Container maxWidth="md" sx={{ py: 6, textAlign: 'center' }}>
      <Typography variant="h3" component="h1" gutterBottom>
        Premedical
      </Typography>
      <Typography variant="h6" color="text.secondary">
        Nauka komunikacji z pacjentem w medycynie ratunkowej.
      </Typography>
    </Container>
  );
}

export default Home;
