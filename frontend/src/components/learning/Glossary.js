import { useEffect, useState } from 'react';
import { Accordion, AccordionDetails, AccordionSummary, List, ListItem, ListItemText, Typography } from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import MenuBookOutlinedIcon from '@mui/icons-material/MenuBookOutlined';
import { getDictionary } from '../../network/lib/dictionary';

function Glossary({ entityId, langId }) {
  const [entries, setEntries] = useState([]);

  useEffect(() => {
    getDictionary(entityId, langId)
      .then(setEntries)
      .catch(() => setEntries([]));
  }, [entityId, langId]);

  if (entries.length === 0) {
    return null;
  }

  return (
    <Accordion sx={{ mt: 3 }}>
      <AccordionSummary expandIcon={<ExpandMoreIcon />}>
        <MenuBookOutlinedIcon sx={{ mr: 1 }} color="secondary" />
        <Typography>Słownik ({entries.length})</Typography>
      </AccordionSummary>
      <AccordionDetails>
        <List dense>
          {entries.map((entry) => (
            <ListItem key={entry.id} disableGutters>
              <ListItemText primary={entry.term} secondary={entry.definition}
                primaryTypographyProps={{ fontWeight: 'bold' }} />
            </ListItem>
          ))}
        </List>
      </AccordionDetails>
    </Accordion>
  );
}

export default Glossary;
