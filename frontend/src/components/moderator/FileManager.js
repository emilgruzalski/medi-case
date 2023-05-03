import { useRef, useState } from 'react';
import { Alert, Box, Button, IconButton, List, ListItem, ListItemIcon, ListItemText, Typography } from '@mui/material';
import AttachFileIcon from '@mui/icons-material/AttachFile';
import AudioFileOutlinedIcon from '@mui/icons-material/AudioFileOutlined';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import ImageOutlinedIcon from '@mui/icons-material/ImageOutlined';
import AudioPlayer from '../learning/AudioPlayer';
import { deleteFile, uploadFile } from '../../network/lib/files';
import { getErrorMessage } from '../../lib/errors';
import { formatFileSize } from '../../lib/utility';

const ACCEPTED_FILES = 'audio/*,image/*';

function FileManager({ translation, onChanged }) {
  const inputRef = useRef(null);
  const [uploading, setUploading] = useState(false);
  const [error, setError] = useState('');

  const handleUpload = async (event) => {
    const file = event.target.files[0];
    if (!file) {
      return;
    }
    setUploading(true);
    try {
      await uploadFile(translation.id, file);
      setError('');
      onChanged();
    } catch (err) {
      setError(getErrorMessage(err));
    } finally {
      setUploading(false);
      event.target.value = '';
    }
  };

  const handleDelete = async (fileId) => {
    try {
      await deleteFile(fileId);
      onChanged();
    } catch (err) {
      setError(getErrorMessage(err));
    }
  };

  return (
    <Box>
      <Typography variant="subtitle1" gutterBottom>
        Pliki
      </Typography>
      {error && <Alert severity="error" sx={{ mb: 1 }}>{error}</Alert>}
      <AudioPlayer files={translation.files} />
      <List dense>
        {translation.files.map((file) => (
          <ListItem key={file.id} secondaryAction={
            <IconButton edge="end" size="small" onClick={() => handleDelete(file.id)}>
              <DeleteOutlineIcon fontSize="small" />
            </IconButton>
          }>
            <ListItemIcon sx={{ minWidth: 32 }}>
              {file.contentType.startsWith('audio/') ? <AudioFileOutlinedIcon /> : <ImageOutlinedIcon />}
            </ListItemIcon>
            <ListItemText primary={file.fileName} secondary={formatFileSize(file.size)} />
          </ListItem>
        ))}
      </List>
      <input ref={inputRef} type="file" accept={ACCEPTED_FILES} hidden onChange={handleUpload} />
      <Button size="small" startIcon={<AttachFileIcon />} onClick={() => inputRef.current.click()} disabled={uploading}>
        Dodaj plik
      </Button>
    </Box>
  );
}

export default FileManager;
