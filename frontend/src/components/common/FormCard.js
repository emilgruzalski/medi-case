import { Container, Paper, Typography } from '@mui/material';

function FormCard({ title, children }) {
  return (
    <Container maxWidth="xs" sx={{ py: 6 }}>
      <Paper sx={{ p: 4 }}>
        <Typography variant="h5" component="h1" gutterBottom>
          {title}
        </Typography>
        {children}
      </Paper>
    </Container>
  );
}

export default FormCard;
