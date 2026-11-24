import { useState } from 'react';
import { Alert, Box, Button, LinearProgress, Paper, Stack, Typography } from '@mui/material';
import CheckIcon from '@mui/icons-material/Check';
import CloseIcon from '@mui/icons-material/Close';
import ReplayIcon from '@mui/icons-material/Replay';
import { calculateScore } from '../../lib/testScore';

function answerColor(answer, chosenId) {
  if (chosenId === undefined) {
    return 'inherit';
  }
  if (answer.isCorrect) {
    return 'success';
  }
  return answer.id === chosenId ? 'error' : 'inherit';
}

function TestQuestion({ number, question, chosenId, onAnswer }) {
  const answered = chosenId !== undefined;

  return (
    <Paper sx={{ p: 2 }}>
      <Typography variant="subtitle1" fontWeight="bold" gutterBottom>
        {number}. {question.title}
      </Typography>
      <Stack spacing={1}>
        {question.children
          .filter((answer) => answer.hasTranslation)
          .map((answer) => (
            <Button
              key={answer.id}
              variant={answered && (answer.isCorrect || answer.id === chosenId) ? 'contained' : 'outlined'}
              color={answerColor(answer, chosenId)}
              onClick={() => !answered && onAnswer(question.id, answer.id)}
              endIcon={answered && answer.isCorrect ? <CheckIcon /> : answered && answer.id === chosenId ? <CloseIcon /> : null}
              sx={{ justifyContent: 'space-between', textTransform: 'none' }}
            >
              {answer.title}
            </Button>
          ))}
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
      <Typography variant="h4" component="h1" gutterBottom>
        {test.title}
      </Typography>
      <Box sx={{ mb: 3 }}>
        <Typography variant="body2" color="text.secondary">
          Odpowiedzi: {answeredCount} / {questions.length}
        </Typography>
        <LinearProgress variant="determinate" value={questions.length ? (answeredCount / questions.length) * 100 : 0} />
      </Box>
      <Stack spacing={2}>
        {questions.map((question, index) => (
          <TestQuestion key={question.id} number={index + 1} question={question} chosenId={answers[question.id]}
            onAnswer={(questionId, answerId) => setAnswers({ ...answers, [questionId]: answerId })} />
        ))}
      </Stack>
      {finished && (
        <Alert severity={score.percent >= 50 ? 'success' : 'warning'} sx={{ mt: 3 }}
          action={<Button color="inherit" startIcon={<ReplayIcon />} onClick={() => setAnswers({})}>Spróbuj ponownie</Button>}>
          Twój wynik: {score.correct} / {score.total} ({score.percent}%)
        </Alert>
      )}
    </>
  );
}

export default LanguageTest;
