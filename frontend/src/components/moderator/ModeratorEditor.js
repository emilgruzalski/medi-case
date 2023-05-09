import { useCallback, useEffect, useState } from 'react';
import ReactQuill from 'react-quill';
import 'react-quill/dist/quill.snow.css';
import { Alert, Box, Button, Chip, Divider, Paper, Snackbar, Stack, Tab, Tabs, TextField, Typography } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import Content from '../learning/Content';
import FileManager from './FileManager';
import DictionaryEditor from './DictionaryEditor';
import TranslateAIButton from './TranslateAIButton';
import SoundAIButton from './SoundAIButton';
import EntitySettings from './EntitySettings';
import { getEntity } from '../../network/lib/entity';
import { createTranslation, updateTranslation } from '../../network/lib/translation';
import { getErrorMessage } from '../../lib/errors';
import { EntityTypes, findTranslation, isBlankHtml, typeLabel } from '../../lib/utility';

const QUILL_MODULES = {
  toolbar: [
    [{ header: [2, 3, false] }],
    ['bold', 'italic', 'underline'],
    [{ list: 'ordered' }, { list: 'bullet' }],
    ['link', 'image'],
    ['clean'],
  ],
};

const EDIT_TAB = 0;
const PREVIEW_TAB = 1;

function ModeratorEditor({ entityId, langId, languages, onSaved }) {
  const [entity, setEntity] = useState(null);
  const [title, setTitle] = useState('');
  const [content, setContent] = useState('');
  const [tab, setTab] = useState(EDIT_TAB);
  const [error, setError] = useState('');
  const [saved, setSaved] = useState(false);

  const load = useCallback(() => {
    getEntity(entityId)
      .then((loaded) => {
        const translation = findTranslation(loaded.translations, langId);
        setEntity(loaded);
        setTitle(translation?.title ?? '');
        setContent(translation?.content ?? '');
        setError('');
      })
      .catch((err) => setError(getErrorMessage(err)));
  }, [entityId, langId]);

  useEffect(load, [load]);

  if (!entity) {
    return error ? <Alert severity="error">{error}</Alert> : null;
  }

  const translation = findTranslation(entity.translations, langId);

  const handleSave = async () => {
    const body = { title: title.trim(), content: isBlankHtml(content) ? null : content };
    try {
      if (translation) {
        await updateTranslation(translation.id, body);
      } else {
        await createTranslation({ entityId, langId, ...body });
      }
      setSaved(true);
      load();
      onSaved();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  return (
    <Paper sx={{ p: 3 }}>
      <Stack direction="row" spacing={1} alignItems="center" sx={{ mb: 2 }}>
        <Typography variant="h6" sx={{ flexGrow: 1 }}>
          {translation?.title ?? entity.translations[0]?.title ?? 'Bez tytułu'}
        </Typography>
        <Chip label={typeLabel(entity.typeId)} size="small" />
        {!translation && <Chip label="Brak tłumaczenia" color="warning" size="small" />}
      </Stack>
      <Stack direction="row" spacing={1} sx={{ mb: 2 }}>
        <TranslateAIButton entity={entity} langId={langId} languages={languages}
          onDone={() => { load(); onSaved(); }} />
        <SoundAIButton translation={translation} onDone={load} />
      </Stack>
      <EntitySettings entity={entity} onChanged={() => { load(); onSaved(); }}
        onError={(err) => setError(getErrorMessage(err))} />
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <TextField label="Tytuł" value={title} onChange={(e) => setTitle(e.target.value)} fullWidth sx={{ mb: 2 }} />
      <Tabs value={tab} onChange={(e, value) => setTab(value)} sx={{ mb: 1 }}>
        <Tab label="Edycja" />
        <Tab label="Podgląd" />
      </Tabs>
      {tab === EDIT_TAB && <ReactQuill theme="snow" value={content} onChange={setContent} modules={QUILL_MODULES} />}
      {tab === PREVIEW_TAB && (
        <Box sx={{ p: 2, border: 1, borderColor: 'divider', borderRadius: 1 }}>
          <Typography variant="h5" gutterBottom>{title}</Typography>
          <Content html={content} />
        </Box>
      )}
      <Button variant="contained" startIcon={<SaveIcon />} onClick={handleSave} disabled={!title.trim()} sx={{ mt: 2 }}>
        Zapisz
      </Button>
      {translation && (
        <>
          <Divider sx={{ my: 3 }} />
          <FileManager translation={translation} onChanged={load} />
        </>
      )}
      {entity.typeId === EntityTypes.Category && (
        <>
          <Divider sx={{ my: 3 }} />
          <DictionaryEditor entityId={entity.id} langId={langId} />
        </>
      )}
      <Snackbar open={saved} autoHideDuration={3000} onClose={() => setSaved(false)} message="Zapisano" />
    </Paper>
  );
}

export default ModeratorEditor;
