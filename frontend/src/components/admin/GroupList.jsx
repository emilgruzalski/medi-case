import { useCallback, useEffect, useState } from 'react';
import { Alert, Button, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import GroupModal from './GroupModal';
import RowActions from './RowActions';
import Pagination from './Pagination';
import AdminSearch from './AdminSearch';
import useDebouncedValue from '../../lib/useDebouncedValue';
import ConfirmDialog from '../common/ConfirmDialog';
import { deleteGroup, getGroups } from '../../network/lib/group';
import { getErrorMessage } from '../../lib/errors';
import { ADMIN_PAGE_SIZE } from '../../lib/admin';

function GroupList() {
  const [result, setResult] = useState(null);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const debouncedSearch = useDebouncedValue(search.trim());
  const [error, setError] = useState('');
  const [editing, setEditing] = useState(undefined);
  const [toDelete, setToDelete] = useState(null);

  const load = useCallback(() => {
    getGroups({ page, pageSize: ADMIN_PAGE_SIZE, search: debouncedSearch || undefined })
      .then(setResult)
      .catch((err) => setError(getErrorMessage(err)));
  }, [page, debouncedSearch]);

  useEffect(() => setPage(1), [debouncedSearch]);

  useEffect(load, [load]);

  const handleDelete = async () => {
    try {
      await deleteGroup(toDelete.id);
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
        Dodaj grupę
      </Button>
      <AdminSearch value={search} onChange={setSearch} placeholder="Szukaj grupy" />
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Nazwa</TableCell>
              <TableCell>Opis</TableCell>
              <TableCell align="right">Użytkownicy</TableCell>
              <TableCell align="right" />
            </TableRow>
          </TableHead>
          <TableBody>
            {result?.items.map((group) => (
              <TableRow key={group.id} hover>
                <TableCell>{group.name}</TableCell>
                <TableCell>{group.description}</TableCell>
                <TableCell align="right">{group.usersCount}</TableCell>
                <TableCell align="right" sx={{ whiteSpace: 'nowrap' }}>
                  <RowActions onEdit={() => setEditing(group)} onDelete={() => setToDelete(group)} />
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
      <Pagination result={result} onPageChange={setPage} />
      <GroupModal open={editing !== undefined} group={editing} onClose={() => setEditing(undefined)}
        onSaved={() => { setEditing(undefined); load(); }} />
      <ConfirmDialog open={Boolean(toDelete)} title="Usunąć grupę?"
        message={`Grupa „${toDelete?.name}” zostanie usunięta. Konta użytkowników zostaną zachowane.`}
        onConfirm={handleDelete} onClose={() => setToDelete(null)} />
    </>
  );
}

export default GroupList;
