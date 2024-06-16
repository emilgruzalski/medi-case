import { Pagination as MuiPagination, Stack, Typography } from '@mui/material';

function Pagination({ result, onPageChange }) {
  if (!result) {
    return null;
  }

  return (
    <Stack direction="row" justifyContent="space-between" alignItems="center" sx={{ mt: 2 }}>
      <Typography variant="body2" color="text.secondary">
        Wyniki: {result.totalCount}
      </Typography>
      {result.totalPages > 1 && (
        <MuiPagination count={result.totalPages} page={result.page} color="primary" shape="rounded"
          onChange={(e, page) => onPageChange(page)} />
      )}
    </Stack>
  );
}

export default Pagination;
