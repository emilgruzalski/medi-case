import { Link as RouterLink } from 'react-router-dom';
import { Card, CardActionArea, CardContent, Grid, Stack, Typography } from '@mui/material';
import ContentComponent from './ContentComponent';
import Glossary from './Glossary';
import QuestionsList from './QuestionsList';
import useStore from '../../state/store';
import { EntityTypes, typeIcon, typeLabel } from '../../lib/utility';

function CategoryView({ node }) {
  const learningLangId = useStore((state) => state.learningLangId);

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
      <Glossary entityId={node.id} langId={learningLangId} />
    </>
  );
}

function ContentContainer({ node }) {
  switch (node.typeId) {
    case EntityTypes.Category:
      return <CategoryView node={node} />;
    case EntityTypes.QuestionList:
      return <QuestionsList node={node} />;
    default:
      return <ContentComponent node={node} />;
  }
}

export default ContentContainer;
