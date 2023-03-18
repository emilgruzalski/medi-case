import { useState } from 'react';
import {
  Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, FormControl, InputLabel, MenuItem, Select, Stack,
  TextField,
} from '@mui/material';
import { createEntity } from '../network/lib/entity';
import { getErrorMessage } from '../lib/errors';
import { EntityTypes, typeLabel } from '../lib/utility';

function AddNodeDialog({ open, parent, langId, onClose, onCreated }) {
  const [typeId, setTypeId] = useState(EntityTypes.Category);
  const [title, setTitle] = useState('');
  const [error, setError] = useState('');

  const handleCreate = async () => {
    try {
      const entity = await createEntity({
        typeId,
        parentId: parent?.id ?? null,
        translations: [{ langId, title: title.trim() }],
      });
      setTitle('');
      setError('');
      onCreated(entity);
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="xs">
      <DialogTitle>{parent ? `Dodaj do: ${parent.title}` : 'Nowy dział główny'}</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}
          <FormControl fullWidth>
            <InputLabel>Typ</InputLabel>
            <Select label="Typ" value={typeId} onChange={(e) => setTypeId(e.target.value)}>
              {Object.values(EntityTypes).map((id) => (
                <MenuItem key={id} value={id}>
                  {typeLabel(id)}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField label="Tytuł" value={title} onChange={(e) => setTitle(e.target.value)} autoFocus fullWidth />
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Anuluj</Button>
        <Button variant="contained" onClick={handleCreate} disabled={!title.trim()}>
          Dodaj
        </Button>
      </DialogActions>
    </Dialog>
  );
}

export default AddNodeDialog;
