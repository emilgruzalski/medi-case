import { useEffect, useState } from 'react';
import { Alert, Button, CircularProgress, Dialog, DialogActions, DialogContent, DialogTitle } from '@mui/material';
import Content from '../learning/Content';
import { getEntity } from '../../network/lib/entity';
import { getErrorMessage } from '../../lib/errors';
import { findTranslation } from '../../lib/utility';

function EntityPreview({ entityId, langId, onClose }) {
  const [entity, setEntity] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!entityId) {
      return;
    }
    setEntity(null);
    getEntity(entityId)
      .then(setEntity)
      .catch((err) => setError(getErrorMessage(err)));
  }, [entityId]);

  const translation = entity ? findTranslation(entity.translations, langId) : undefined;

  return (
    <Dialog open={Boolean(entityId)} onClose={onClose} fullWidth maxWidth="md">
      <DialogTitle>{translation?.title ?? 'Podgląd'}</DialogTitle>
      <DialogContent dividers>
        {error && <Alert severity="error">{error}</Alert>}
        {!entity && !error && <CircularProgress />}
        {entity && !translation && <Alert severity="info">Brak tłumaczenia w tym języku.</Alert>}
        {translation && <Content html={translation.content} />}
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Zamknij</Button>
      </DialogActions>
    </Dialog>
  );
}

export default EntityPreview;
