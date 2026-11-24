import { useState } from 'react';
import { Box, Button, Paper, Stack, Typography } from '@mui/material';
import { calculateScore } from '../../lib/testScore';

const ANSWER = {
  justifyContent: 'space-between', gap: 2, minHeight: 52, px: 2, textAlign: 'left', fontWeight: 500,
  // Answers stay readable after the question is answered.
  '&.Mui-disabled': { color: 'text.secondary', borderColor: 'divider' },
};

function answerState(answer, chosenId) {
  if (chosenId === undefined) {
    return { sx: { color: 'text.primary', borderColor: '#8b9bad' } };
  }
  if (answer.isCorrect) {
    return {
      note: answer.id === chosenId ? 'dobrze' : 'poprawna',
      sx: { '&.Mui-disabled': { bgcolor: 'success.main', color: '#ffffff', borderColor: 'success.main' } },
    };
  }
  return answer.id === chosenId
    ? { note: 'Twoja odpowiedź', sx: { '&.Mui-disabled': { color: 'error.main', border: 2, borderColor: 'error.main' } } }
    : { sx: {} };
}

function TestQuestion({ number, question, chosenId, onAnswer }) {
  const answered = chosenId !== undefined;

  return (
    <Paper component="section" sx={{ p: { xs: 2.5, md: 4 } }}>
      <Typography variant="h6" component="h2" sx={{ mb: 2 }}>
        {number}. {question.title}
      </Typography>
      <Stack spacing={1}>
        {question.children
          .filter((answer) => answer.hasTranslation)
          .map((answer) => {
            const state = answerState(answer, chosenId);
            return (
              <Button key={answer.id} variant="outlined" color="inherit" disabled={answered}
                onClick={() => onAnswer(question.id, answer.id)} sx={[ANSWER, state.sx]}>
                <span>{answer.title}</span>
                {state.note && <Typography component="span" variant="body2" sx={{ flex: 'none' }}>{state.note}</Typography>}
              </Button>
            );
          })}
      </Stack>
    </Paper>
  );
}

function LanguageTest({ test }) {
  const [answers, setAnswers] = useState({});
  const questions = test.children.filter((question) => question.hasTranslation);
  const answeredCount = Object.keys(answers).length;
  const finished = questions.length > 0 && answeredCount === questions.length;
  const score = calculateScore(questions, answers);

  return (
    <>
      <Box sx={{ mb: 2, p: { xs: 2.5, md: 4 }, bgcolor: 'primary.main', color: 'primary.contrastText', borderRadius: 2 }}>
        <Typography variant="h4" component="h1">{test.title}</Typography>
        <Typography sx={{ mt: 0.5, color: '#d3deea' }} aria-live="polite">
          Odpowiedzi: {answeredCount} / {questions.length}
        </Typography>
      </Box>
      <Stack spacing={2}>
        {questions.map((question, index) => (
          <TestQuestion key={question.id} number={index + 1} question={question} chosenId={answers[question.id]}
            onAnswer={(questionId, answerId) => setAnswers({ ...answers, [questionId]: answerId })} />
        ))}
      </Stack>
      {finished && (
        <Stack role="status" direction="row" spacing={2} useFlexGap
          sx={{ mt: 3, alignItems: 'center', justifyContent: 'space-between', flexWrap: 'wrap' }}>
          <Typography variant="h5" component="p">Twój wynik: {score.correct} / {score.total} ({score.percent}%)</Typography>
          <Button size="large" variant="contained" color="secondary" onClick={() => setAnswers({})}>Spróbuj ponownie</Button>
        </Stack>
      )}
    </>
  );
}

export default LanguageTest;
