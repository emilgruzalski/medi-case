import { Box, Container, Typography } from '@mui/material';

function Footer() {
  return (
    <Box component="footer" sx={{ py: 3 }}>
      <Container maxWidth="lg">
        <Typography variant="body2" color="text.secondary" align="center">
          MediCase, materiały do nauki przygotowane we współpracy z Collegium Medicum UMK
        </Typography>
      </Container>
    </Box>
  );
}

export default Footer;
