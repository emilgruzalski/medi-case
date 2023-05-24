import { useState } from 'react';
import { Box, Paper, Tab, Tabs, Typography } from '@mui/material';
import UserList from './UserList';
import GroupList from './GroupList';
import ModeratorReview from '../user/ModeratorReview';

const TABS = [
  { label: 'Użytkownicy', component: <UserList /> },
  { label: 'Grupy', component: <GroupList /> },
  { label: 'Publikacja', component: <ModeratorReview /> },
];

function BoardAdmin() {
  const [tab, setTab] = useState(0);

  return (
    <Box sx={{ p: { xs: 2, md: 3 } }}>
      <Typography variant="h5" component="h1" gutterBottom>
        Panel administratora
      </Typography>
      <Paper sx={{ mb: 2 }}>
        <Tabs value={tab} onChange={(e, value) => setTab(value)} variant="scrollable">
          {TABS.map((item) => <Tab key={item.label} label={item.label} />)}
        </Tabs>
      </Paper>
      {TABS[tab].component}
    </Box>
  );
}

export default BoardAdmin;
