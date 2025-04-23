import { useEffect, useState } from 'react';
import { Navigate, useParams } from 'react-router';
import { Alert, Box, Button, CircularProgress, Drawer, Grid, Typography, useMediaQuery } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import MenuIcon from '@mui/icons-material/Menu';
import Sidebar from './Sidebar';
import ContentContainer from './ContentContainer';
import ContentNavigation from './ContentNavigation';
import SearchBox from './SearchBox';
import useStore from '../../state/store';
import { getLearningTree } from '../../network/lib/learning';
import { getErrorMessage } from '../../lib/errors';
import { findNode } from '../../lib/utility';

function Learning() {
  const { entityId } = useParams();
  const learningLangId = useStore((state) => state.learningLangId);
  const [tree, setTree] = useState(null);
  const [error, setError] = useState('');
  const [drawerOpen, setDrawerOpen] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  useEffect(() => {
    if (!learningLangId) {
      return;
    }
    getLearningTree(learningLangId)
      .then(setTree)
      .catch((err) => setError(getErrorMessage(err)));
  }, [learningLangId]);

  if (!learningLangId) {
    return <Navigate to="/language" replace />;
  }

  const node = tree ? findNode(tree, entityId) : undefined;

  return (
    <Box sx={{ p: { xs: 2, md: 3 } }}>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      {!tree && !error && <CircularProgress />}
      {tree && (
        <Grid container spacing={3}>
          {isMobile ? (
            <Grid size={12}>
              <Button startIcon={<MenuIcon />} variant="outlined" onClick={() => setDrawerOpen(true)}>
                Spis treści
              </Button>
              <Drawer open={drawerOpen} onClose={() => setDrawerOpen(false)}>
                <Box sx={{ width: 300, p: 1 }}>
                  <SearchBox onSelected={() => setDrawerOpen(false)} />
                  <Box onClick={() => setDrawerOpen(false)}>
                    <Sidebar tree={tree} selectedId={entityId} />
                  </Box>
                </Box>
              </Drawer>
            </Grid>
          ) : (
            <Grid size={{ md: 4, lg: 3 }}>
              <SearchBox />
              <Sidebar tree={tree} selectedId={entityId} />
            </Grid>
          )}
          <Grid size={{ xs: 12, md: 8, lg: 9 }}>
            {node ? (
              <>
                <ContentContainer node={node} />
                <ContentNavigation tree={tree} node={node} />
              </>
            ) : (
              <Typography color="text.secondary">
                {isMobile ? 'Otwórz spis treści, aby wybrać temat.' : 'Wybierz temat z listy po lewej stronie.'}
              </Typography>
            )}
          </Grid>
        </Grid>
      )}
    </Box>
  );
}

export default Learning;
