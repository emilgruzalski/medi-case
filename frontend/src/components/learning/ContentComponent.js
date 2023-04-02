import { useState } from 'react';
import { Box, Divider, FormControlLabel, Paper, Switch, Typography } from '@mui/material';
import Content from './Content';
import useStore from '../../state/store';
import { findTranslation } from '../../lib/utility';

function ContentComponent({ entity }) {
  const { learningLangId, nativeLangId } = useStore();
  const [showTranslation, setShowTranslation] = useState(false);
  const translation = findTranslation(entity.translations, learningLangId);
  const nativeTranslation = findTranslation(entity.translations, nativeLangId);
  const canTranslate = nativeTranslation && nativeLangId !== learningLangId;

  if (!translation) {
    return <Typography color="text.secondary">Ta treść nie jest jeszcze dostępna w wybranym języku.</Typography>;
  }

  return (
    <Paper sx={{ p: 3 }}>
      <Typography variant="h5" component="h2" gutterBottom>
        {translation.title}
      </Typography>
      <Content html={translation.content} />
      {canTranslate && (
        <>
          <FormControlLabel
            sx={{ mt: 2 }}
            control={<Switch checked={showTranslation} onChange={(e) => setShowTranslation(e.target.checked)} />}
            label="Pokaż tłumaczenie"
          />
          {showTranslation && (
            <Box sx={{ mt: 1, color: 'text.secondary' }}>
              <Divider sx={{ mb: 2 }} />
              <Typography variant="h6">{nativeTranslation.title}</Typography>
              <Content html={nativeTranslation.content} />
            </Box>
          )}
        </>
      )}
    </Paper>
  );
}

export default ContentComponent;
