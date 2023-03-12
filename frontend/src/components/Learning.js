import { useEffect, useState } from 'react';
import { Navigate, useParams } from 'react-router-dom';
import { Alert, Box, CircularProgress, Grid, Typography } from '@mui/material';
import Sidebar from './Sidebar';
import ContentContainer from './ContentContainer';
import useStore from '../state/store';
import { getTree } from '../network/lib/entity';
import { getErrorMessage } from '../lib/errors';
import { findNode } from '../lib/utility';

function Learning() {
  const { entityId } = useParams();
  const learningLangId = useStore((state) => state.learningLangId);
  const [tree, setTree] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    if (!learningLangId) {
      return;
    }
    getTree(learningLangId)
      .then(setTree)
      .catch((err) => setError(getErrorMessage(err)));
  }, [learningLangId]);

  if (!learningLangId) {
    return <Navigate to="/language" replace />;
  }

  const node = tree ? findNode(tree, entityId) : undefined;

  return (
    <Box sx={{ p: 3 }}>
      {error && <Alert severity="error" sx={{ mb: 2 }}>{error}</Alert>}
      {!tree && !error && <CircularProgress />}
      {tree && (
        <Grid container spacing={3}>
          <Grid item xs={12} md={4} lg={3}>
            <Sidebar tree={tree} selectedId={entityId} />
          </Grid>
          <Grid item xs={12} md={8} lg={9}>
            {node ? (
              <ContentContainer node={node} />
            ) : (
              <Typography color="text.secondary">Wybierz temat z listy po lewej stronie.</Typography>
            )}
          </Grid>
        </Grid>
      )}
    </Box>
  );
}

export default Learning;
