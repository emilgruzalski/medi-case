import { useEffect, useRef, useState } from 'react';
import { Button, Typography } from '@mui/material';
import StopIcon from '@mui/icons-material/Stop';
import VolumeUpIcon from '@mui/icons-material/VolumeUp';
import { getLearningFileObjectUrl } from '../../network/lib/learning';

/** Plays a single recording. The file is downloaded when it is needed for the first time. */
function ListenButton({ file, sx }) {
  const audioRef = useRef(null);
  const urlRef = useRef(null);
  const [loading, setLoading] = useState(false);
  const [playing, setPlaying] = useState(false);
  const [failed, setFailed] = useState(false);

  useEffect(() => {
    const audio = audioRef.current;
    return () => {
      audio.pause();
      if (urlRef.current) {
        URL.revokeObjectURL(urlRef.current);
      }
    };
  }, []);

  const handleClick = async () => {
    const audio = audioRef.current;
    if (playing) {
      audio.pause();
      audio.currentTime = 0;
      return;
    }

    setFailed(false);
    setLoading(true);
    try {
      if (!urlRef.current) {
        urlRef.current = await getLearningFileObjectUrl(file.id);
        audio.src = urlRef.current;
      }
      await audio.play();
    } catch {
      setFailed(true);
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      <audio ref={audioRef} onPlay={() => setPlaying(true)} onPause={() => setPlaying(false)} onEnded={() => setPlaying(false)} />
      <Button variant="contained" color="secondary" disabled={loading} onClick={handleClick}
        startIcon={playing ? <StopIcon /> : <VolumeUpIcon />} sx={sx}>
        {playing ? 'Zatrzymaj' : 'Odsłuchaj'}
      </Button>
      {failed && (
        <Typography role="alert" sx={{ order: 9, flexBasis: '100%' }}>Nie udało się odtworzyć nagrania. Spróbuj ponownie.</Typography>
      )}
    </>
  );
}

export default ListenButton;
