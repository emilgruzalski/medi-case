import { useState } from 'react';
import { Alert, Box, CircularProgress, Divider, FormControlLabel, Paper, Switch, Typography } from '@mui/material';
import AudioPlayer from './AudioPlayer';
import Content from './Content';
import useEntityInLanguages from '../../lib/useEntityInLanguages';

function ContentComponent({ node }) {
  const { learning, native, error } = useEntityInLanguages(node.id);
  const [showTranslation, setShowTranslation] = useState(false);

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }
  if (!learning) {
    return <CircularProgress />;
  }
  if (!learning.hasTranslation) {
    return <Typography color="text.secondary">Ta treść nie jest jeszcze dostępna w wybranym języku.</Typography>;
  }

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h5" component="h2" gutterBottom>
        {learning.title}
      </Typography>
      <AudioPlayer files={learning.files} />
      <Content html={learning.content} />
      {native?.hasTranslation && (
        <>
          <FormControlLabel
            sx={{ mt: 2 }}
            control={<Switch checked={showTranslation} onChange={(e) => setShowTranslation(e.target.checked)} />}
            label="Pokaż tłumaczenie"
          />
          {showTranslation && (
            <Box sx={{ mt: 1, color: 'text.secondary' }}>
              <Divider sx={{ mb: 2 }} />
              <Typography variant="h6">{native.title}</Typography>
              <AudioPlayer files={native.files} />
              <Content html={native.content} />
            </Box>
          )}
        </>
      )}
    </Paper>
  );
}

export default ContentComponent;
