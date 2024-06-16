import { useCallback, useEffect, useState } from 'react';
import {
  Alert, Button, Chip, FormControl, InputLabel, List, ListItem, ListItemIcon, ListItemText, MenuItem, Paper, Select,
  Stack, Typography,
} from '@mui/material';
import PublishIcon from '@mui/icons-material/Publish';
import VisibilityOutlinedIcon from '@mui/icons-material/VisibilityOutlined';
import ConfirmDialog from '../common/ConfirmDialog';
import EntityPreview from './EntityPreview';
import { getLanguages } from '../../network/lib/language';
import { getPendingChanges, publish } from '../../network/lib/sync';
import { getErrorMessage } from '../../lib/errors';
import { typeIcon, typeLabel } from '../../lib/utility';

const CHANGE_LABELS = { Added: 'Nowe', Modified: 'Zmienione', Removed: 'Usunięte' };
const CHANGE_COLORS = { Added: 'success', Modified: 'warning', Removed: 'error' };

function ModeratorReview() {
  const [languages, setLanguages] = useState([]);
  const [langId, setLangId] = useState('');
  const [changes, setChanges] = useState(null);
  const [previewId, setPreviewId] = useState(null);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    getLanguages()
      .then((list) => {
        setLanguages(list);
        setLangId(list[0]?.id ?? '');
      })
      .catch((err) => setError(getErrorMessage(err)));
  }, []);

  const load = useCallback(() => {
    if (!langId) {
      return;
    }
    getPendingChanges(langId)
      .then(setChanges)
      .catch((err) => setError(getErrorMessage(err)));
  }, [langId]);

  useEffect(load, [load]);

  const handlePublish = async () => {
    setConfirmOpen(false);
    try {
      setResult(await publish());
      setError('');
      load();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  return (
    <>
      <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 2 }}>
        <FormControl size="small" sx={{ minWidth: 180 }}>
          <InputLabel>Język podglądu</InputLabel>
          <Select label="Język podglądu" value={langId} onChange={(e) => setLangId(e.target.value)}>
            {languages.map((language) => <MenuItem key={language.id} value={language.id}>{language.name}</MenuItem>)}
          </Select>
        </FormControl>
        <Button variant="contained" startIcon={<PublishIcon />} disabled={!changes?.hasChanges}
          onClick={() => setConfirmOpen(true)}>
          Opublikuj zmiany
        </Button>
      </Stack>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      {result && (
        <Alert severity="success" sx={{ mb: 2 }} onClose={() => setResult(null)}>
          Opublikowano {result.entities} elementów, {result.translations} tłumaczeń i {result.files} plików.
        </Alert>
      )}
      {changes?.languagesChanged && <Alert severity="info" sx={{ mb: 2 }}>Lista języków została zmieniona.</Alert>}
      <Paper>
        {changes && changes.entities.length === 0 && (
          <Typography color="text.secondary" sx={{ p: 2 }}>Brak zmian do opublikowania.</Typography>
        )}
        <List dense>
          {changes?.entities.map((change) => (
            <ListItem key={change.entityId} secondaryAction={
              change.changeType !== 'Removed' && (
                <Button size="small" startIcon={<VisibilityOutlinedIcon />} onClick={() => setPreviewId(change.entityId)}>
                  Podgląd
                </Button>
              )
            }>
              <ListItemIcon sx={{ minWidth: 32 }}>{typeIcon(change.typeId)}</ListItemIcon>
              <ListItemText primary={change.title}
                secondary={`${typeLabel(change.typeId)} · ${new Date(change.modifiedAt).toLocaleString('pl-PL')}`} />
              <Chip size="small" sx={{ mr: 12 }} label={CHANGE_LABELS[change.changeType]}
                color={CHANGE_COLORS[change.changeType]} />
            </ListItem>
          ))}
        </List>
      </Paper>
      <EntityPreview entityId={previewId} langId={langId} onClose={() => setPreviewId(null)} />
      <ConfirmDialog open={confirmOpen} title="Opublikować zmiany?" confirmLabel="Opublikuj"
        message="Uczniowie zobaczą aktualną wersję wszystkich treści przygotowanych przez moderatorów."
        onConfirm={handlePublish} onClose={() => setConfirmOpen(false)} />
    </>
  );
}

export default ModeratorReview;
