import { useEffect, useState } from 'react';
import { Box, Button, Stack } from '@mui/material';
import VolumeUpIcon from '@mui/icons-material/VolumeUp';
import { getLearningFileObjectUrl } from '../../network/lib/learning';

function AudioFile({ file, loadUrl }) {
  const [url, setUrl] = useState(null);
  const [loading, setLoading] = useState(false);

  useEffect(() => () => url && URL.revokeObjectURL(url), [url]);

  const handlePlay = async () => {
    setLoading(true);
    try {
      setUrl(await loadUrl(file.id));
    } finally {
      setLoading(false);
    }
  };

  if (url) {
    return <Box component="audio" controls autoPlay src={url} sx={{ height: 36 }} />;
  }
  return (
    <Button size="small" variant="outlined" startIcon={<VolumeUpIcon />} onClick={handlePlay} disabled={loading}>
      Odsłuchaj
    </Button>
  );
}

function AudioPlayer({ files, loadUrl = getLearningFileObjectUrl }) {
  const audioFiles = (files ?? []).filter((file) => file.contentType.startsWith('audio/'));
  if (audioFiles.length === 0) {
    return null;
  }

  return (
    <Stack direction="row" spacing={1} sx={{ my: 1 }} flexWrap="wrap">
      {audioFiles.map((file) => (
        <AudioFile key={file.id} file={file} loadUrl={loadUrl} />
      ))}
    </Stack>
  );
}

export default AudioPlayer;
