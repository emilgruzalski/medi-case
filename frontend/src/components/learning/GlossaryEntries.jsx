import { Box, Paper, Typography } from '@mui/material';

function GlossaryEntries({ entries }) {
  return (
    <Paper component="dl" sx={{ m: 0, px: { xs: 2.5, md: 3 } }}>
      {entries.map((entry) => (
        <Box key={entry.id} sx={{ py: 2, '& + &': { borderTop: 1, borderColor: 'divider' } }}>
          <Typography component="dt" variant="h6">{entry.term}</Typography>
          <Typography component="dd" color="text.secondary" sx={{ m: 0, maxWidth: '68ch' }}>{entry.definition}</Typography>
        </Box>
      ))}
    </Paper>
  );
}

export default GlossaryEntries;
