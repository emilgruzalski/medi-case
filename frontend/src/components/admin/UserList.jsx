import { useCallback, useEffect, useState } from 'react';
import {
  Alert, Button, Chip, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import UserModal from './UserModal';
import RowActions from './RowActions';
import Pagination from './Pagination';
import AdminSearch from './AdminSearch';
import useDebouncedValue from '../../lib/useDebouncedValue';
import ConfirmDialog from '../common/ConfirmDialog';
import { deleteUser, getUsers } from '../../network/lib/user';
import { getErrorMessage } from '../../lib/errors';
import { ADMIN_PAGE_SIZE, ROLE_COLORS, ROLE_LABELS } from '../../lib/admin';

function UserList() {
  const [result, setResult] = useState(null);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const debouncedSearch = useDebouncedValue(search.trim());
  const [error, setError] = useState('');
  const [editing, setEditing] = useState(undefined);
  const [toDelete, setToDelete] = useState(null);

  const load = useCallback(() => {
    getUsers({ page, pageSize: ADMIN_PAGE_SIZE, search: debouncedSearch || undefined })
      .then(setResult)
      .catch((err) => setError(getErrorMessage(err)));
  }, [page, debouncedSearch]);

  useEffect(() => setPage(1), [debouncedSearch]);

  useEffect(load, [load]);

  const handleDelete = async () => {
    try {
      await deleteUser(toDelete.id);
      load();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setToDelete(null);
    }
  };

  return (
    <>
      <Button startIcon={<AddIcon />} variant="contained" sx={{ mb: 2 }} onClick={() => setEditing(null)}>
        Zaproś użytkownika
      </Button>
      <AdminSearch value={search} onChange={setSearch} placeholder="Szukaj po imieniu, nazwisku lub e-mailu" />
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Imię i nazwisko</TableCell>
              <TableCell>E-mail</TableCell>
              <TableCell>Rola</TableCell>
              <TableCell>Grupy</TableCell>
              <TableCell align="right" />
            </TableRow>
          </TableHead>
          <TableBody>
            {result?.items.map((user) => (
              <TableRow key={user.id} hover>
                <TableCell>{user.firstName} {user.lastName}</TableCell>
                <TableCell>{user.email}</TableCell>
                <TableCell>
                  <Chip size="small" label={ROLE_LABELS[user.role]} color={ROLE_COLORS[user.role]} />
                </TableCell>
                <TableCell>
                  <Stack direction="row" spacing={0.5} flexWrap="wrap">
                    {user.groups.map((group) => <Chip key={group.id} size="small" variant="outlined" label={group.name} />)}
                  </Stack>
                </TableCell>
                <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                  <RowActions onEdit={() => setEditing(user)} onDelete={() => setToDelete(user)} />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <Pagination result={result} onPageChange={setPage} />
      <UserModal open={editing !== undefined} user={editing} onClose={() => setEditing(undefined)}
        onSaved={() => { setEditing(undefined); load(); }} />
      <ConfirmDialog open={Boolean(toDelete)} title="Usunąć użytkownika?"
        message={`Konto ${toDelete?.email} zostanie usunięte.`} onConfirm={handleDelete} onClose={() => setToDelete(null)} />
    </>
  );
}

export default UserList;
