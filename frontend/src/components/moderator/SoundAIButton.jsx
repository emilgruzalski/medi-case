import { useState } from 'react';
import { Button, CircularProgress } from '@mui/material';
import RecordVoiceOverIcon from '@mui/icons-material/RecordVoiceOver';
import { generateSpeech } from '../../network/lib/ai';

function SoundAIButton({ translation, onDone, onError }) {
  const [loading, setLoading] = useState(false);

  const handleClick = async () => {
    setLoading(true);
    try {
      await generateSpeech(translation.id);
      onDone();
    } catch (err) {
      onError(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <Button size="small" variant="outlined" onClick={handleClick} disabled={!translation || loading}
      startIcon={loading ? <CircularProgress size={16} /> : <RecordVoiceOverIcon />}>
      Generuj nagranie
    </Button>
  );
}

export default SoundAIButton;
