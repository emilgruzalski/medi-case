import { useState } from 'react';
import { Alert, Avatar, Box, Button, CircularProgress, Collapse, Paper, Stack, Typography } from '@mui/material';
import TranslateIcon from '@mui/icons-material/Translate';
import AudioPlayer from './AudioPlayer';
import Content from './Content';
import useEntityInLanguages from '../../lib/useEntityInLanguages';

function QuestionCard({ number, question, translation }) {
  const [showTranslation, setShowTranslation] = useState(false);

  return (
    <Paper sx={{ p: 2 }}>
      <Stack direction="row" spacing={2} alignItems="flex-start">
        <Avatar sx={{ bgcolor: 'secondary.main', width: 32, height: 32, fontSize: 16 }}>{number}</Avatar>
        <Box sx={{ flexGrow: 1 }}>
          <Typography variant="h6">{question.title}</Typography>
          <AudioPlayer files={question.files} />
          <Box sx={{ color: 'text.secondary' }}>
            <Content html={question.content} />
          </Box>
          {translation?.hasTranslation && (
            <>
              <Button size="small" startIcon={<TranslateIcon />} onClick={() => setShowTranslation(!showTranslation)}>
                {showTranslation ? 'Ukryj tłumaczenie' : 'Tłumaczenie'}
              </Button>
              <Collapse in={showTranslation}>
                <Box sx={{ mt: 1, pl: 2, borderLeft: 3, borderColor: 'secondary.light' }}>
                  <Typography fontWeight="bold">{translation.title}</Typography>
                  <AudioPlayer files={translation.files} />
                  <Content html={translation.content} />
                </Box>
              </Collapse>
            </>
          )}
        </Box>
      </Stack>
    </Paper>
  );
}

function QuestionsList({ node }) {
  const { learning, native, error } = useEntityInLanguages(node.id);

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }
  if (!learning) {
    return <CircularProgress />;
  }

  const questions = learning.children.filter((question) => question.hasTranslation);

  return (
    <>
      <Typography variant="h4" component="h1" gutterBottom>
        {learning.title}
      </Typography>
      <Typography color="text.secondary" sx={{ mb: 3 }}>
        Pytania, które warto zadać pacjentowi. Przeczytaj je na głos i porównaj z tłumaczeniem.
      </Typography>
      <Stack spacing={2}>
        {questions.map((question, index) => (
          <QuestionCard key={question.id} number={index + 1} question={question}
            translation={native?.children.find((child) => child.id === question.id)} />
        ))}
      </Stack>
    </>
  );
}

export default QuestionsList;
