import { useNavigate } from 'react-router-dom';
import { Button, Stack } from '@mui/material';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import { findSiblings } from '../../lib/utility';

function ContentNavigation({ tree, node }) {
  const navigate = useNavigate();
  const siblings = findSiblings(tree, node.id) ?? [];
  const index = siblings.findIndex((sibling) => sibling.id === node.id);
  const previous = siblings[index - 1];
  const next = siblings[index + 1];

  if (!previous && !next) {
    return null;
  }

  return (
    <Stack direction="row" justifyContent="space-between" sx={{ mt: 3 }}>
      <span>
        {previous && (
          <Button startIcon={<ArrowBackIcon />} onClick={() => navigate(`/learning/${previous.id}`)}>
            {previous.title}
          </Button>
        )}
      </span>
      <span>
        {next && (
          <Button endIcon={<ArrowForwardIcon />} onClick={() => navigate(`/learning/${next.id}`)}>
            {next.title}
          </Button>
        )}
      </span>
    </Stack>
  );
}

export default ContentNavigation;
