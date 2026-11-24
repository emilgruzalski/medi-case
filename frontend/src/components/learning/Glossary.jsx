import { useEffect, useState } from 'react';
import { Box, Typography } from '@mui/material';
import GlossaryEntries from './GlossaryEntries';
import { getLearningDictionary } from '../../network/lib/learning';

/** Glossary of a single category; nothing is shown when it is empty. */
function Glossary({ entityId, langId, title }) {
  const [entries, setEntries] = useState([]);

  useEffect(() => {
    let active = true;
    setEntries([]);
    getLearningDictionary(entityId, langId)
      .then((loaded) => active && setEntries(loaded))
      .catch(() => active && setEntries([]));
    return () => {
      active = false;
    };
  }, [entityId, langId]);

  if (entries.length === 0) {
    return null;
  }

  return (
    <Box component="section" sx={{ mt: 4 }}>
      <Typography variant="h5" component="h2" sx={{ mb: 1.5 }}>{title}</Typography>
      <GlossaryEntries entries={entries} />
    </Box>
  );
}

export default Glossary;
