import { useState } from 'react';
import { Button, CircularProgress, Menu, MenuItem, Snackbar } from '@mui/material';
import AutoAwesomeIcon from '@mui/icons-material/AutoAwesome';
import ConfirmDialog from '../common/ConfirmDialog';
import { translateWithAi } from '../../network/lib/ai';
import { getErrorMessage } from '../../lib/errors';

function TranslateAIButton({ entity, langId, languages, onDone }) {
  const [anchorEl, setAnchorEl] = useState(null);
  const [source, setSource] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const sources = entity.translations.filter((translation) => translation.langId !== langId);
  const hasTarget = entity.translations.some((translation) => translation.langId === langId);
  const languageName = (id) => languages.find((language) => language.id === id)?.name ?? id;

  const translate = async (translation) => {
    setSource(null);
    setLoading(true);
    try {
      await translateWithAi(translation.id, langId);
      onDone();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  const handleChoose = (translation) => {
    setAnchorEl(null);
    if (hasTarget) {
      setSource(translation);
    } else {
      translate(translation);
    }
  };

  return (
    <>
      <Button size="small" variant="outlined" disabled={sources.length === 0 || loading}
        startIcon={loading ? <CircularProgress size={16} /> : <AutoAwesomeIcon />}
        onClick={(e) => setAnchorEl(e.currentTarget)}>
        Tłumacz z AI
      </Button>
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={() => setAnchorEl(null)}>
        {sources.map((translation) => (
          <MenuItem key={translation.id} onClick={() => handleChoose(translation)}>
            z języka: {languageName(translation.langId)}
          </MenuItem>
        ))}
      </Menu>
      <ConfirmDialog open={Boolean(source)} title="Nadpisać tłumaczenie?" confirmLabel="Nadpisz"
        message="Obecny tytuł i treść w tym języku zostaną zastąpione tłumaczeniem wygenerowanym przez AI."
        onConfirm={() => translate(source)} onClose={() => setSource(null)} />
      <Snackbar open={Boolean(error)} autoHideDuration={5000} onClose={() => setError('')} message={error} />
    </>
  );
}

export default TranslateAIButton;
