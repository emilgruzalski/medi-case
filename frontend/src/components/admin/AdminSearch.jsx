import { InputAdornment, TextField } from '@mui/material';
import SearchIcon from '@mui/icons-material/Search';

function AdminSearch({ value, onChange, placeholder }) {
  return (
    <TextField size="small" value={value} placeholder={placeholder} onChange={(e) => onChange(e.target.value)}
      sx={{ mb: 2, minWidth: 260 }}
      InputProps={{ startAdornment: <InputAdornment position="start"><SearchIcon /></InputAdornment> }} />
  );
}

export default AdminSearch;
