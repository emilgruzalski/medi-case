import { Alert, Typography } from '@mui/material';
import ContentComponent from './ContentComponent';
import Glossary from './Glossary';
import PatientInterview from './PatientInterview';
import LanguageTest from './LanguageTest';
import TopicList from './TopicList';
import PageLoader from '../common/PageLoader';
import useEntityInLanguages from '../../lib/useEntityInLanguages';
import useStore from '../../state/store';
import { EntityTypes } from '../../lib/utility';

function CategoryView({ node }) {
  const learningLangId = useStore((state) => state.learningLangId);

  return (
    <>
      <Typography variant="h4" component="h1" sx={{ mb: 2 }}>{node.title}</Typography>
      <TopicList nodes={node.children} />
      <Glossary entityId={node.id} langId={learningLangId} title="Słownik działu" />
    </>
  );
}

function TestView({ node }) {
  const { learning, error } = useEntityInLanguages(node.id);

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }
  return learning ? <LanguageTest key={learning.id} test={learning} /> : <PageLoader />;
}

function ContentContainer({ node }) {
  switch (node.typeId) {
    case EntityTypes.Category:
      return <CategoryView node={node} />;
    case EntityTypes.QuestionList:
      return <PatientInterview key={node.id} node={node} />;
    case EntityTypes.Test:
      return <TestView node={node} />;
    default:
      return <ContentComponent node={node} />;
  }
}

export default ContentContainer;
