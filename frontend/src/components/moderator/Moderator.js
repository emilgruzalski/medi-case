import { useCallback, useEffect, useState } from 'react';
import {
  Alert, Box, Button, FormControl, Grid, InputLabel, MenuItem, Select, Stack, ToggleButton, ToggleButtonGroup, Typography,
  useMediaQuery,
} from '@mui/material';
import { useTheme } from '@mui/material/styles';
import ModeratorTree from './ModeratorTree';
import ModeratorEditor from './ModeratorEditor';
import LanguageList from './LanguageList';
import useStore from '../../state/store';
import { getTree } from '../../network/lib/entity';
import { getLanguages } from '../../network/lib/language';
import { getEntityTypes } from '../../network/lib/entityType';
import { getErrorMessage } from '../../lib/errors';

const TREE_VIEW = 'tree';
const EDITOR_VIEW = 'editor';

function Moderator() {
  const { moderatorLangId, setModeratorLangId } = useStore();
  const [languages, setLanguages] = useState([]);
  const [tree, setTree] = useState([]);
  const [types, setTypes] = useState([]);
  const [selectedId, setSelectedId] = useState(null);
  const [error, setError] = useState('');
  const [languagesOpen, setLanguagesOpen] = useState(false);
  const [mobileView, setMobileView] = useState(TREE_VIEW);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));
  const showTree = !isMobile || mobileView === TREE_VIEW;
  const showEditor = !isMobile || mobileView === EDITOR_VIEW;

  const loadLanguages = useCallback(() => {
    getLanguages()
      .then((list) => {
        setLanguages(list);
        if (!list.some((language) => language.id === moderatorLangId) && list.length > 0) {
          setModeratorLangId(list[0].id);
        }
      })
      .catch((err) => setError(getErrorMessage(err)));
  }, [moderatorLangId, setModeratorLangId]);

  useEffect(loadLanguages, [loadLanguages]);

  useEffect(() => {
    getEntityTypes()
      .then(setTypes)
      .catch((err) => setError(getErrorMessage(err)));
  }, []);

  const loadTree = useCallback(() => {
    if (!moderatorLangId) {
      return;
    }
    getTree(moderatorLangId)
      .then(setTree)
      .catch((err) => setError(getErrorMessage(err)));
  }, [moderatorLangId]);

  useEffect(loadTree, [loadTree]);

  const handleChanged = (newSelectedId) => {
    if (newSelectedId) {
      setSelectedId(newSelectedId);
    }
    loadTree();
  };

  const handleSelect = (node) => {
    setSelectedId(node.id);
    setMobileView(EDITOR_VIEW);
  };

  const handleDeleted = (deletedId) => {
    if (deletedId === selectedId) {
      setSelectedId(null);
    }
    loadTree();
  };

  return (
    <Box sx={{ p: { xs: 2, md: 3 } }}>
      <Stack direction="row" spacing={2} alignItems="center" sx={{ mb: 3, rowGap: 1 }} flexWrap="wrap">
        <Typography variant="h5" component="h1" sx={{ flexGrow: 1 }}>
          Zarządzanie treścią
        </Typography>
        <FormControl size="small" sx={{ minWidth: 180 }}>
          <InputLabel>Język edycji</InputLabel>
          <Select label="Język edycji" value={moderatorLangId ?? ''} onChange={(e) => setModeratorLangId(e.target.value)}>
            {languages.map((language) => (
              <MenuItem key={language.id} value={language.id}>
                {language.name}
              </MenuItem>
            ))}
          </Select>
        </FormControl>
        <Button variant="outlined" onClick={() => setLanguagesOpen(true)}>
          Języki
        </Button>
      </Stack>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      {isMobile && (
        <ToggleButtonGroup exclusive fullWidth size="small" value={mobileView} sx={{ mb: 2 }}
          onChange={(e, value) => value && setMobileView(value)}>
          <ToggleButton value={TREE_VIEW}>Drzewo</ToggleButton>
          <ToggleButton value={EDITOR_VIEW} disabled={!selectedId}>Edycja</ToggleButton>
        </ToggleButtonGroup>
      )}
      <Grid container spacing={3}>
        {showTree && (
          <Grid item xs={12} md={5} lg={4}>
            <ModeratorTree tree={tree} types={types} langId={moderatorLangId} selectedId={selectedId}
              onSelect={handleSelect} onChanged={handleChanged} onDeleted={handleDeleted} />
          </Grid>
        )}
        {showEditor && (
          <Grid item xs={12} md={7} lg={8}>
            {selectedId && moderatorLangId ? (
              <ModeratorEditor entityId={selectedId} langId={moderatorLangId} languages={languages} onSaved={loadTree} />
            ) : (
              <Typography color="text.secondary">Wybierz element drzewa, aby go edytować.</Typography>
            )}
          </Grid>
        )}
      </Grid>
      <LanguageList open={languagesOpen} languages={languages} onClose={() => setLanguagesOpen(false)}
        onChanged={loadLanguages} />
    </Box>
  );
}

export default Moderator;
