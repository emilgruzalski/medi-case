import { useEffect, useState } from 'react';
import { Alert, Button, Dialog, DialogActions, DialogContent, DialogTitle, Stack, TextField } from '@mui/material';
import { createGroup, updateGroup } from '../../network/lib/group';
import { getErrorMessage } from '../../lib/errors';

function GroupModal({ open, group, onClose, onSaved }) {
  const [name, setName] = useState('');
  const [description, setDescription] = useState('');
  const [error, setError] = useState('');

  useEffect(() => {
    if (open) {
      setName(group?.name ?? '');
      setDescription(group?.description ?? '');
      setError('');
    }
  }, [open, group]);

  const handleSave = async () => {
    const data = { name: name.trim(), description: description.trim() || null };
    try {
      if (group) {
        await updateGroup(group.id, data);
      } else {
        await createGroup(data);
      }
      onSaved();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="xs">
      <DialogTitle>{group ? 'Edytuj grupę' : 'Nowa grupa'}</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}
          <TextField label="Nazwa" value={name} onChange={(e) => setName(e.target.value)} fullWidth autoFocus />
          <TextField label="Opis" value={description} onChange={(e) => setDescription(e.target.value)}
            fullWidth multiline minRows={2} />
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Anuluj</Button>
        <Button variant="contained" onClick={handleSave} disabled={!name.trim()}>Zapisz</Button>
      </DialogActions>
    </Dialog>
  );
}

export default GroupModal;
