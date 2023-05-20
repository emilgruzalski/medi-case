import { useCallback, useEffect, useState } from 'react';
import { Alert, Paper, Table, TableBody, TableCell, TableContainer, TableHead, TableRow } from '@mui/material';
import { getGroups } from '../../network/lib/group';
import { getErrorMessage } from '../../lib/errors';
import { ADMIN_PAGE_SIZE } from '../../lib/admin';

function GroupList() {
  const [result, setResult] = useState(null);
  const [error, setError] = useState('');

  const load = useCallback(() => {
    getGroups({ page: 1, pageSize: ADMIN_PAGE_SIZE })
      .then(setResult)
      .catch((err) => setError(getErrorMessage(err)));
  }, []);

  useEffect(load, [load]);

  return (
    <>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <TableContainer component={Paper}>
        <Table size="small">
          <TableHead>
            <TableRow>
              <TableCell>Nazwa</TableCell>
              <TableCell>Opis</TableCell>
              <TableCell align="right">Użytkownicy</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {result?.items.map((group) => (
              <TableRow key={group.id} hover>
                <TableCell>{group.name}</TableCell>
                <TableCell>{group.description}</TableCell>
                <TableCell align="right">{group.usersCount}</TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
}

export default GroupList;
