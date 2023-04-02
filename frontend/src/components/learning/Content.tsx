import DOMPurify from 'dompurify';
import { Box } from '@mui/material';

interface ContentProps {
  html: string | null | undefined;
}

function Content({ html }: ContentProps) {
  if (!html) {
    return null;
  }

  // Content comes from the rich text editor of moderators, so it is sanitized before rendering.
  return <Box className="content-html" dangerouslySetInnerHTML={{ __html: DOMPurify.sanitize(html) }} />;
}

export default Content;
