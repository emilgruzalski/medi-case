import { useEffect, useState } from 'react';
import {
  Alert, Button, Chip, Dialog, DialogActions, DialogContent, DialogTitle, FormControl, InputLabel, MenuItem,
  OutlinedInput, Select, Stack, TextField,
} from '@mui/material';
import { createUser, updateUser } from '../../network/lib/user';
import { getGroups } from '../../network/lib/group';
import { getErrorMessage } from '../../lib/errors';
import { GROUP_OPTIONS_LIMIT, ROLE_LABELS } from '../../lib/admin';
import { Roles } from '../../lib/roles';

const EMPTY_USER = { firstName: '', lastName: '', email: '', role: Roles.User, groupIds: [] };

function toForm(user) {
  return user
    ? { ...EMPTY_USER, firstName: user.firstName, lastName: user.lastName, email: user.email, role: user.role,
        groupIds: user.groups.map((group) => group.id) }
    : EMPTY_USER;
}

function UserModal({ open, user, onClose, onSaved }) {
  const [form, setForm] = useState(EMPTY_USER);
  const [groups, setGroups] = useState([]);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!open) {
      return;
    }
    setForm(toForm(user));
    setError('');
    getGroups({ page: 1, pageSize: GROUP_OPTIONS_LIMIT })
      .then((result) => setGroups(result.items))
      .catch((err) => setError(getErrorMessage(err)));
  }, [open, user]);

  const field = (name) => ({
    value: form[name],
    onChange: (e) => setForm({ ...form, [name]: e.target.value }),
    fullWidth: true,
  });

  const handleSave = async () => {
    try {
      if (user) {
        await updateUser(user.id, form);
      } else {
        await createUser(form);
      }
      onSaved();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  return (
    <Dialog open={open} onClose={onClose} fullWidth maxWidth="sm">
      <DialogTitle>{user ? 'Edytuj użytkownika' : 'Zaproś użytkownika'}</DialogTitle>
      <DialogContent>
        <Stack spacing={2} sx={{ mt: 1 }}>
          {error && <Alert severity="error">{error}</Alert>}
          <Stack direction={{ xs: 'column', sm: 'row' }} spacing={2}>
            <TextField label="Imię" {...field('firstName')} />
            <TextField label="Nazwisko" {...field('lastName')} />
          </Stack>
          <TextField label="E-mail" type="email" {...field('email')} />
          {!user && (
            <Alert severity="info">
              Użytkownik dostanie e-mail z hasłem tymczasowym i ustawi własne hasło przy pierwszym logowaniu.
            </Alert>
          )}
          <FormControl fullWidth>
            <InputLabel>Rola</InputLabel>
            <Select label="Rola" {...field('role')}>
              {Object.values(Roles).map((role) => <MenuItem key={role} value={role}>{ROLE_LABELS[role]}</MenuItem>)}
            </Select>
          </FormControl>
          <FormControl fullWidth>
            <InputLabel>Grupy</InputLabel>
            <Select multiple input={<OutlinedInput label="Grupy" />} {...field('groupIds')}
              renderValue={(ids) => (
                <Stack direction="row" spacing={0.5} flexWrap="wrap">
                  {ids.map((id) => <Chip key={id} size="small" label={groups.find((g) => g.id === id)?.name ?? id} />)}
                </Stack>
              )}>
              {groups.map((group) => <MenuItem key={group.id} value={group.id}>{group.name}</MenuItem>)}
            </Select>
          </FormControl>
        </Stack>
      </DialogContent>
      <DialogActions>
        <Button onClick={onClose}>Anuluj</Button>
        <Button variant="contained" onClick={handleSave}>Zapisz</Button>
      </DialogActions>
    </Dialog>
  );
}

export default UserModal;
