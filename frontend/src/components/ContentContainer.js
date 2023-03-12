import { useEffect, useState } from 'react';
import { Link as RouterLink } from 'react-router-dom';
import { Alert, Card, CardActionArea, CardContent, CircularProgress, Grid, Stack, Typography } from '@mui/material';
import ContentComponent from './ContentComponent';
import { getEntity } from '../network/lib/entity';
import { getErrorMessage } from '../lib/errors';
import { EntityTypes, typeIcon, typeLabel } from '../lib/utility';

function CategoryView({ node }) {
  return (
    <>
      <Typography variant="h4" component="h1" gutterBottom>
        {node.title}
      </Typography>
      <Grid container spacing={2}>
        {node.children.map((child) => (
          <Grid item xs={12} sm={6} key={child.id}>
            <Card>
              <CardActionArea component={RouterLink} to={`/learning/${child.id}`}>
                <CardContent>
                  <Stack direction="row" spacing={1} alignItems="center" color="text.secondary">
                    {typeIcon(child.typeId)}
                    <Typography variant="caption">{typeLabel(child.typeId)}</Typography>
                  </Stack>
                  <Typography variant="h6">{child.title}</Typography>
                </CardContent>
              </CardActionArea>
            </Card>
          </Grid>
        ))}
      </Grid>
    </>
  );
}

function ContentContainer({ node }) {
  const [entity, setEntity] = useState(null);
  const [error, setError] = useState('');

  useEffect(() => {
    setEntity(null);
    setError('');
    getEntity(node.id)
      .then(setEntity)
      .catch((err) => setError(getErrorMessage(err)));
  }, [node.id]);

  if (node.typeId === EntityTypes.Category) {
    return <CategoryView node={node} />;
  }
  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }
  if (!entity) {
    return <CircularProgress />;
  }
  return <ContentComponent entity={entity} />;
}

export default ContentContainer;
