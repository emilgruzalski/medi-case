import { useState } from 'react';
import { Alert, Box, FormControlLabel, Paper, Switch, Typography } from '@mui/material';
import AudioPlayer from './AudioPlayer';
import Content from './Content';
import PageLoader from '../common/PageLoader';
import useEntityInLanguages from '../../lib/useEntityInLanguages';

function ContentComponent({ node }) {
  const { learning, native, error } = useEntityInLanguages(node.id);
  const [showTranslation, setShowTranslation] = useState(false);

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }
  if (!learning) {
    return <PageLoader />;
  }
  if (!learning.hasTranslation) {
    return <Typography color="text.secondary">Ta treść nie jest jeszcze dostępna w wybranym języku.</Typography>;
  }

  return (
    <Paper sx={{ overflow: 'hidden' }}>
      <Box sx={{ px: { xs: 2.5, md: 5 }, py: { xs: 3, md: 4.5 }, fontSize: { xs: '1.0625rem', md: '1.1875rem' } }}>
        <Typography variant="h4" component="h1" sx={{ mb: 2, fontSize: { xs: '1.75rem', md: '2.25rem' } }}>{learning.title}</Typography>
        <AudioPlayer files={learning.files} />
        <Content html={learning.content} />
        {native?.hasTranslation && (
          <FormControlLabel sx={{ mt: 2 }} label="Pokaż tłumaczenie"
            control={<Switch checked={showTranslation} onChange={(e) => setShowTranslation(e.target.checked)} />} />
        )}
      </Box>
      {native?.hasTranslation && showTranslation && (
        <Box sx={{ px: { xs: 2.5, md: 5 }, py: { xs: 3, md: 4 }, bgcolor: 'primary.main', color: 'primary.contrastText', fontSize: '1.0625rem' }}>
          <Typography variant="h5" component="h2" sx={{ mb: 1.5 }}>{native.title}</Typography>
          <AudioPlayer files={native.files} />
          <Content html={native.content} />
        </Box>
      )}
    </Paper>
  );
}

export default ContentComponent;
