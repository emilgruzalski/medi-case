import { useState } from 'react';
import {
  Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, IconButton, List, ListItem, ListItemText, Stack,
  TextField,
} from '@mui/material';
import CheckIcon from '@mui/icons-material/Check';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import { createLanguage, deleteLanguage, updateLanguage } from '../../network/lib/language';
import { getErrorMessage } from '../../lib/errors';

const EMPTY_LANGUAGE = { langValue: '', name: '' };

function LanguageList({ open, languages, onClose, onChanged }) {
  const [editingId, setEditingId] = useState(null);
  const [form, setForm] = useState(EMPTY_LANGUAGE);
  const [error, setError] = useState('');

  const run = async (action) => {
    try {
      await action();
      setError('');
      setEditingId(null);
      setForm(EMPTY_LANGUAGE);
      onChanged();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  const startEditing = (language) => {
    setEditingId(language.id);
    setForm({ langValue: language.langValue, name: language.name });
  };

  const handleSubmit = () =>
    run(() => (editingId ? updateLanguage(editingId, form) : createLanguage(form)));

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>Języki treści</DialogTitle>
      <DialogContent>
        {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
        <List dense>
          {languages.map((language) => (
            <ListItem
              key={language.id}
              selected={language.id === editingId}
              secondaryAction={
                <>
                  <IconButton size="small" onClick={() => startEditing(language)}>
                    <EditOutlinedIcon fontSize="small" />
                  </IconButton>
                  <IconButton size="small" onClick={() => run(() => deleteLanguage(language.id))}>
                    <DeleteOutlineIcon fontSize="small" />
                  </IconButton>
                </>
              }
            >
              <ListItemText primary={language.name} secondary={language.langValue} />
            </ListItem>
          ))}
        </List>
        <Stack direction="row" spacing={1} sx={{ mt: 2 }}>
          <TextField label="Kod (np. en)" size="small" value={form.langValue}
            onChange={(e) => setForm({ ...form, langValue: e.target.value })} />
          <TextField label="Nazwa" size="small" value={form.name} sx={{ flexGrow: 1 }}
            onChange={(e) => setForm({ ...form, name: e.target.value })} />
          <IconButton color="primary" onClick={handleSubmit} disabled={!form.langValue.trim() || !form.name.trim()}>
            <CheckIcon />
          </IconButton>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Zamknij</Button>
      </DialogActions>
    </Dialog>
  );
}

export default LanguageList;
