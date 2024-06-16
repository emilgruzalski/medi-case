import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { InputAdornment, List, ListItemButton, ListItemIcon, ListItemText, Paper, TextField, Typography } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import useStore from '../../state/store';
import useDebouncedValue from '../../lib/useDebouncedValue';
import { searchLearning } from '../../network/lib/learning';
import { typeIcon } from '../../lib/utility';

const MIN_QUERY_LENGTH = 2;

function SearchBox({ onSelected }) {
  const learningLangId = useStore((state) => state.learningLangId);
  const [query, setQuery] = useState('');
  const [results, setResults] = useState([]);
  const debouncedQuery = useDebouncedValue(query.trim());
  const navigate = useNavigate();

  useEffect(() => {
    if (debouncedQuery.length < MIN_QUERY_LENGTH) {
      setResults([]);
      return;
    }
    searchLearning(debouncedQuery, learningLangId)
      .then(setResults)
      .catch(() => setResults([]));
  }, [debouncedQuery, learningLangId]);

  const handleSelect = (result) => {
    setQuery('');
    navigate(`/learning/${result.entityId}`);
    onSelected?.();
  };

  return (
    <Paper sx={{ p: 1, mb: 2 }}>
      <TextField fullWidth size="small" placeholder="Szukaj w materiałach" value={query}
        onChange={(e) => setQuery(e.target.value)}
        InputProps={{ startAdornment: <InputAdornment position="start"><SearchIcon /></InputAdornment> }} />
      {debouncedQuery.length >= MIN_QUERY_LENGTH && (
        <List dense>
          {results.length === 0 && (
            <Typography variant="body2" color="text.secondary" sx={{ px: 1, pt: 1 }}>Brak wyników.</Typography>
          )}
          {results.map((result) => (
            <ListItemButton key={result.entityId} onClick={() => handleSelect(result)}>
              <ListItemIcon sx={{ minWidth: 32 }}>{typeIcon(result.typeId)}</ListItemIcon>
              <ListItemText primary={result.title} secondary={result.path.join(' › ')} />
            </ListItemButton>
          ))}
        </List>
      )}
    </Paper>
  );
}

export default SearchBox;
