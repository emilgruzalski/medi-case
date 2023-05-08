import { useState } from 'react';
import { Button, CircularProgress, Snackbar } from '@mui/material';
import RecordVoiceOverIcon from '@mui/icons-material/RecordVoiceOver';
import { generateSpeech } from '../../network/lib/ai';
import { getErrorMessage } from '../../lib/errors';

function SoundAIButton({ translation, onDone }) {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const handleClick = async () => {
    setLoading(true);
    try {
      await generateSpeech(translation.id);
      onDone();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <Button size="small" variant="outlined" onClick={handleClick} disabled={!translation || loading}
        startIcon={loading ? <CircularProgress size={16} /> : <RecordVoiceOverIcon />}>
        Generuj nagranie
      </Button>
      <Snackbar open={Boolean(error)} autoHideDuration={5000} onClose={() => setError('')} message={error} />
    </>
  );
}

export default SoundAIButton;
