import { useEffect, useState } from 'react';
import { Navigate } from 'react-router';
import { Alert, Box, Container, InputAdornment, Stack, TextField, Typography } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';
import GlossaryEntries from './GlossaryEntries';
import PageLoader from '../common/PageLoader';
import useStore from '../../state/store';
import useLearningTree from '../../lib/useLearningTree';
import { getLearningDictionary } from '../../network/lib/learning';
import { getErrorMessage } from '../../lib/errors';
import { collectCategories } from '../../lib/topics';

const matches = (entry, query) =>
  `${entry.term} ${entry.definition}`.toLocaleLowerCase('pl').includes(query.trim().toLocaleLowerCase('pl'));

/** Glossaries of all categories in one place. */
function GlossaryPage() {
  const learningLangId = useStore((state) => state.learningLangId);
  const { tree, error: treeError } = useLearningTree();
  const [sections, setSections] = useState(null);
  const [error, setError] = useState('');
  const [query, setQuery] = useState('');

  useEffect(() => {
    let active = true;
    if (!tree) {
      return undefined;
    }
    Promise.all(collectCategories(tree).map((category) =>
      getLearningDictionary(category.id, learningLangId).then((entries) => ({ category, entries }))))
      .then((loaded) => active && setSections(loaded))
      .catch((err) => active && setError(getErrorMessage(err)));
    return () => {
      active = false;
    };
  }, [tree, learningLangId]);

  if (!learningLangId) {
    return <Navigate to="/language" replace />;
  }
  if (treeError || error) {
    return <Container sx={{ py: 4 }}><Alert severity="error">{treeError || error}</Alert></Container>;
  }
  if (!sections) {
    return <PageLoader />;
  }

  const visible = sections
    .map(({ category, entries }) => ({ category, entries: entries.filter((entry) => matches(entry, query)) }))
    .filter(({ entries }) => entries.length > 0);

  return (
    <Container maxWidth="md" sx={{ py: { xs: 2.5, md: 5 } }}>
      <Typography variant="h4" component="h1" sx={{ mb: 2 }}>Słownik</Typography>
      <TextField fullWidth placeholder="Szukaj pojęcia" value={query} onChange={(e) => setQuery(e.target.value)} sx={{ mb: 4 }}
        slotProps={{
          htmlInput: { 'aria-label': 'Szukaj pojęcia' },
          input: { startAdornment: <InputAdornment position="start"><SearchIcon /></InputAdornment> },
        }} />
      {visible.length === 0 && (
        <Typography color="text.secondary">
          {query ? 'Nie ma takiego pojęcia.' : 'Słownik jest jeszcze pusty w wybranym języku.'}
        </Typography>
      )}
      <Stack spacing={4}>
        {visible.map(({ category, entries }) => (
          <Box component="section" key={category.id}>
            <Typography variant="h5" component="h2" sx={{ mb: 1.5 }}>{category.title}</Typography>
            <GlossaryEntries entries={entries} />
          </Box>
        ))}
      </Stack>
    </Container>
  );
}

export default GlossaryPage;
