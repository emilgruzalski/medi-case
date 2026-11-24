import { Link as RouterLink } from 'react-router';
import { Box, Link, Typography } from '@mui/material';
import TopicList from './TopicList';
import { collectCategories, ownTopics } from '../../lib/topics';

/** Every category of the content tree with its topics. */
function TopicSections({ tree, selectedId, dense = false, columns = 1 }) {
  return (
    <Box sx={{ display: 'grid', gap: dense ? 3 : 4, alignItems: 'start', gridTemplateColumns: { xs: 'minmax(0, 1fr)', md: `repeat(${columns}, minmax(0, 1fr))` } }}>
      {collectCategories(tree).map((category) => (
        <section key={category.id}>
          <Typography variant={dense ? 'h6' : 'h5'} component="h2" sx={{ mb: 1.25 }}>
            <Link component={RouterLink} to={`/learning/${category.id}`} color="inherit" underline="hover"
              aria-current={category.id === selectedId ? 'page' : undefined}>
              {category.title}
            </Link>
          </Typography>
          <TopicList nodes={ownTopics(category)} selectedId={selectedId} dense={dense} />
        </section>
      ))}
    </Box>
  );
}

export default TopicSections;
