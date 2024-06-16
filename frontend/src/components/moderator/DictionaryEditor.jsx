import { useCallback, useEffect, useState } from 'react';
import { Alert, Box, Button, IconButton, List, ListItem, ListItemText, Stack, TextField, Typography } from '@mui/material';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import {
  createDictionaryEntry, deleteDictionaryEntry, getDictionary, updateDictionaryEntry,
} from '../../network/lib/dictionary';
import { getErrorMessage } from '../../lib/errors';

const EMPTY_ENTRY = { term: '', definition: '' };

function DictionaryEditor({ entityId, langId }) {
  const [entries, setEntries] = useState([]);
  const [form, setForm] = useState(EMPTY_ENTRY);
  const [editingId, setEditingId] = useState(null);
  const [error, setError] = useState('');

  const load = useCallback(() => {
    getDictionary(entityId, langId)
      .then(setEntries)
      .catch((err) => setError(getErrorMessage(err)));
  }, [entityId, langId]);

  useEffect(load, [load]);

  const run = async (action) => {
    try {
      await action();
      setForm(EMPTY_ENTRY);
      setEditingId(null);
      setError('');
      load();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  const handleSubmit = () =>
    run(() => (editingId
      ? updateDictionaryEntry(editingId, form)
      : createDictionaryEntry({ entityId, langId, ...form })));

  return (
    <Box>
      <Typography variant="subtitle1" gutterBottom>
        Słownik
      </Typography>
      {error && <Alert severity="error" sx={{ mb: 1 }}>{error}</Alert>}
      <List dense>
        {entries.map((entry) => (
          <ListItem key={entry.id} selected={entry.id === editingId} secondaryAction={
            <>
              <IconButton size="small" onClick={() => {
                setEditingId(entry.id);
                setForm({ term: entry.term, definition: entry.definition });
              }}>
                <EditOutlinedIcon fontSize="small" />
              </IconButton>
              <IconButton size="small" onClick={() => run(() => deleteDictionaryEntry(entry.id))}>
                <DeleteOutlineIcon fontSize="small" />
              </IconButton>
            </>
          }>
            <ListItemText primary={entry.term} secondary={entry.definition} />
          </ListItem>
        ))}
      </List>
      <Stack spacing={1}>
        <TextField label="Hasło" size="small" value={form.term} onChange={(e) => setForm({ ...form, term: e.target.value })} />
        <TextField label="Definicja" size="small" multiline minRows={2} value={form.definition}
          onChange={(e) => setForm({ ...form, definition: e.target.value })} />
        <Stack direction="row" spacing={1}>
          <Button size="small" variant="outlined" onClick={handleSubmit}
            disabled={!form.term.trim() || !form.definition.trim()}>
            {editingId ? 'Zapisz hasło' : 'Dodaj hasło'}
          </Button>
          {editingId && (
            <Button size="small" onClick={() => { setEditingId(null); setForm(EMPTY_ENTRY); }}>
              Anuluj
            </Button>
          )}
        </Stack>
      </Stack>
    </Box>
  );
}

export default DictionaryEditor;
