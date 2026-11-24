import { Link as RouterLink } from 'react-router';
import { Box, Typography } from '@mui/material';
import { EntityTypes } from '../../lib/utility';
import { topicCaption } from '../../lib/topics';

function rowColors(node, selected) {
  if (selected) {
    return { bgcolor: 'primary.main', color: 'primary.contrastText' };
  }
  // Interviews are the heart of the app, so they stand out among the other topics.
  return node.typeId === EntityTypes.QuestionList
    ? { bgcolor: 'secondary.main', color: 'secondary.contrastText' }
    : { bgcolor: 'background.paper', color: 'text.primary' };
}

/** Topics as large rows that are easy to hit with a thumb. */
function TopicList({ nodes, selectedId, dense = false }) {
  return (
    <Box component="ul" sx={{ m: 0, p: 0, listStyle: 'none', display: 'grid', gap: 0.75 }}>
      {nodes.map((node) => {
        const selected = node.id === selectedId;
        return (
          <li key={node.id}>
            <Box component={RouterLink} to={`/learning/${node.id}`} aria-current={selected ? 'page' : undefined}
              sx={{
                display: 'flex', alignItems: 'center', justifyContent: 'space-between', gap: 2,
                minHeight: dense ? 52 : 64, px: 2.25, py: 1, boxSizing: 'border-box', borderRadius: 1, textDecoration: 'none',
                ...rowColors(node, selected),
                '&:hover': { outline: 2, outlineStyle: 'solid', outlineColor: 'primary.main' },
                '&:focus-visible': { outline: 3, outlineStyle: 'solid', outlineColor: 'primary.main', outlineOffset: 2 },
              }}>
              <Typography component="span" fontWeight={500}>{node.title}</Typography>
              <Typography component="span" variant="body2" sx={{ flex: 'none', opacity: 0.85 }}>{topicCaption(node)}</Typography>
            </Box>
          </li>
        );
      })}
    </Box>
  );
}

export default TopicList;
