import { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Alert, Button, FormControl, InputLabel, MenuItem, Select, Stack, Typography } from '@mui/material';
import FormCard from '../common/FormCard';
import useStore from '../../state/store';
import { getLearningLanguages } from '../../network/lib/learning';
import { getErrorMessage } from '../../lib/errors';

function LanguageSelect({ label, value, languages, onChange }) {
  return (
    <FormControl fullWidth>
      <InputLabel>{label}</InputLabel>
      <Select label={label} value={value} onChange={(e) => onChange(e.target.value)}>
        {languages.map((language) => (
          <MenuItem key={language.id} value={language.id}>
            {language.name}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
}

function SetLanguage() {
  const { learningLangId, nativeLangId, setLanguages } = useStore();
  const [languages, setLanguageList] = useState([]);
  const [learning, setLearning] = useState(learningLangId ?? '');
  const [native, setNative] = useState(nativeLangId ?? '');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  useEffect(() => {
    getLearningLanguages()
      .then(setLanguageList)
      .catch((err) => setError(getErrorMessage(err)));
  }, []);

  const handleSave = () => {
    setLanguages(learning, native);
    navigate('/learning');
  };

  return (
    <FormCard title="Wybierz języki">
      <Typography variant="body2" color="text.secondary" sx={{ mb: 3 }}>
        Treści będą wyświetlane w języku nauki. Tłumaczenie na Twój język możesz włączyć przy każdej karcie.
      </Typography>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      <Stack spacing={3}>
        <LanguageSelect label="Język nauki" value={learning} languages={languages} onChange={setLearning} />
        <LanguageSelect label="Twój język" value={native} languages={languages} onChange={setNative} />
        <Button variant="contained" onClick={handleSave} disabled={!learning || !native}>
          Zapisz
        </Button>
      </Stack>
    </FormCard>
  );
}

export default SetLanguage;
