import { Link as RouterLink, Navigate, useParams } from 'react-router';
import { Alert, Box, Button, Container, Typography, useMediaQuery } from '@mui/material';
import { useTheme } from '@mui/material/styles';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import ContentContainer from './ContentContainer';
import ContentNavigation from './ContentNavigation';
import SearchBox from './SearchBox';
import TopicSections from './TopicSections';
import PageLoader from '../common/PageLoader';
import useStore from '../../state/store';
import useLearningTree from '../../lib/useLearningTree';
import { findNode } from '../../lib/utility';

const SIDEBAR_WIDTH = 340;

function Topics({ tree }) {
  return (
    <Container maxWidth="lg" sx={{ py: { xs: 2.5, md: 5 } }}>
      <Typography variant="h4" component="h1" sx={{ mb: 2 }}>Tematy</Typography>
      <Box sx={{ maxWidth: 560, mb: 3 }}><SearchBox /></Box>
      {tree.length === 0
        ? <Typography color="text.secondary">Nie ma jeszcze opublikowanych materiałów.</Typography>
        : <TopicSections tree={tree} columns={2} />}
    </Container>
  );
}

function Topic({ tree, node }) {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down('md'));

  if (!node) {
    return (
      <Container maxWidth="sm" sx={{ py: 5 }}>
        <Alert severity="info" action={<Button color="inherit" component={RouterLink} to="/learning">Tematy</Button>}>
          Tego tematu nie ma w wybranym języku.
        </Alert>
      </Container>
    );
  }

  const content = (
    <>
      <ContentContainer node={node} />
      <ContentNavigation tree={tree} node={node} />
    </>
  );

  if (isMobile) {
    return (
      <Box sx={{ px: 2, pt: 1, pb: 3 }}>
        <Button component={RouterLink} to="/learning" startIcon={<ArrowBackIcon />} sx={{ mb: 1 }}>Tematy</Button>
        {content}
      </Box>
    );
  }

  return (
    <Box sx={{ display: 'grid', gridTemplateColumns: `${SIDEBAR_WIDTH}px minmax(0, 1fr)`, gap: 4, p: 4, alignItems: 'start' }}>
      <Box component="nav" aria-label="Tematy">
        <SearchBox />
        <TopicSections tree={tree} selectedId={node.id} dense />
      </Box>
      <Box sx={{ maxWidth: 1040 }}>{content}</Box>
    </Box>
  );
}

function Learning() {
  const { entityId } = useParams();
  const learningLangId = useStore((state) => state.learningLangId);
  const { tree, error } = useLearningTree();

  if (!learningLangId) {
    return <Navigate to="/language" replace />;
  }
  if (error) {
    return <Container sx={{ py: 4 }}><Alert severity="error">{error}</Alert></Container>;
  }
  if (!tree) {
    return <PageLoader />;
  }
  return entityId ? <Topic tree={tree} node={findNode(tree, entityId)} /> : <Topics tree={tree} />;
}

export default Learning;
