import { useState } from 'react';
import { Alert, Box, Button, Paper, Typography } from '@mui/material';
import ChevronLeftIcon from '@mui/icons-material/ChevronLeft';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import Content from './Content';
import ListenButton from './ListenButton';
import PageLoader from '../common/PageLoader';
import useEntityInLanguages from '../../lib/useEntityInLanguages';

const STEP_BUTTON = {
  flex: '0 0 72px', height: { xs: 80, md: 72 }, minWidth: 0, color: 'inherit', borderColor: '#8fa6c0',
  '&:hover': { borderColor: '#ffffff', bgcolor: 'primary.dark' },
  '&.Mui-disabled': { color: '#6f89a8', borderColor: '#44648a' },
};

const findAudio = (entity) => (entity?.files ?? []).filter((file) => file.contentType.startsWith('audio/'));

function Progress({ index, count }) {
  return (
    <Box aria-hidden="true" sx={{ display: 'grid', gridTemplateColumns: `repeat(${count}, minmax(0, 1fr))`, gap: 0.75, mt: 2 }}>
      {Array.from({ length: count }, (_, i) => (
        <Box key={i} sx={{ height: 6, borderRadius: 3, bgcolor: i <= index ? 'secondary.main' : 'primary.light' }} />
      ))}
    </Box>
  );
}

function Translation({ translation, shown, onToggle }) {
  return (
    <Box sx={{ mt: { xs: 4, md: 5 } }}>
      {shown && (
        <Box id="translation" sx={{ px: 3, py: 2.5, bgcolor: 'primary.dark', borderRadius: 1.5 }}>
          <Typography sx={{ fontSize: { xs: '1.4rem', md: '1.75rem' }, lineHeight: 1.25, fontWeight: 500 }}>{translation.title}</Typography>
          <Box sx={{ color: '#d3deea', '& p': { mb: 0 } }}><Content html={translation.content} /></Box>
          {findAudio(translation).slice(0, 1).map((file) => <ListenButton key={file.id} file={file} sx={{ mt: 2 }} />)}
        </Box>
      )}
      <Button color="inherit" onClick={onToggle} aria-expanded={shown} aria-controls={shown ? 'translation' : undefined}
        sx={{ mt: 1, px: 0.5, textDecoration: 'underline', textUnderlineOffset: 4 }}>
        {shown ? 'Ukryj tłumaczenie' : 'Pokaż tłumaczenie'}
      </Button>
    </Box>
  );
}

/** Questions to ask the patient, one at a time, so each can be heard, repeated aloud and checked. */
function PatientInterview({ node }) {
  const { learning, native, error } = useEntityInLanguages(node.id);
  const [index, setIndex] = useState(0);
  const [showTranslation, setShowTranslation] = useState(false);

  if (error) {
    return <Alert severity="error">{error}</Alert>;
  }
  if (!learning) {
    return <PageLoader />;
  }

  const questions = learning.children.filter((question) => question.hasTranslation);
  if (questions.length === 0) {
    return <Typography color="text.secondary">Ta rozmowa nie ma jeszcze pytań w wybranym języku.</Typography>;
  }

  const question = questions[index];
  const translation = native?.children.find((child) => child.id === question.id);
  const [audio] = findAudio(question);

  return (
    <>
      <Box component="section" aria-label={learning.title}
        sx={{
          display: 'flex', flexDirection: 'column', minHeight: { xs: 'calc(100dvh - 250px)', sm: 560 }, p: { xs: 2.5, md: 5 },
          bgcolor: 'primary.main', color: 'primary.contrastText', borderRadius: 2,
        }}>
        <Box sx={{ display: 'flex', flexWrap: 'wrap', justifyContent: 'space-between', gap: 1 }}>
          <Typography component="h1" fontWeight={500}>{learning.title}</Typography>
          <Typography sx={{ color: '#d3deea' }} aria-live="polite">Pytanie {index + 1} z {questions.length}</Typography>
        </Box>
        <Progress index={index} count={questions.length} />
        <Typography component="h2"
          sx={{ mt: { xs: 4, md: 6 }, fontSize: 'clamp(2.4rem, 5.2vw, 4.25rem)', lineHeight: 1.08, fontWeight: 600, letterSpacing: '-0.02em', overflowWrap: 'anywhere' }}>
          {question.title}
        </Typography>
        <Box sx={{ mt: 1, color: '#d3deea', fontSize: '1.125rem', '& p': { mb: 0 } }}><Content html={question.content} /></Box>
        {translation?.hasTranslation && (
          <Translation translation={translation} shown={showTranslation} onToggle={() => setShowTranslation(!showTranslation)} />
        )}
        <Box sx={{ display: 'flex', flexWrap: 'wrap', alignItems: 'center', gap: 1.5, mt: 'auto', pt: 4 }}>
          <Button variant="outlined" aria-label="Poprzednie pytanie" disabled={index === 0} onClick={() => setIndex(index - 1)} sx={STEP_BUTTON}>
            <ChevronLeftIcon fontSize="large" />
          </Button>
          {audio
            ? <ListenButton key={audio.id} file={audio} sx={{ flex: '1 1 0', maxWidth: { md: 320 }, height: { xs: 80, md: 72 }, fontSize: '1.25rem', borderRadius: 1.5 }} />
            : <Box sx={{ flex: { xs: '1 1 0', md: '0 0 0' } }} />}
          <Button variant="outlined" aria-label="Następne pytanie" disabled={index === questions.length - 1} onClick={() => setIndex(index + 1)} sx={STEP_BUTTON}>
            <ChevronRightIcon fontSize="large" />
          </Button>
        </Box>
      </Box>

      <Paper component="nav" aria-label="Wszystkie pytania" sx={{ mt: 2, overflow: 'hidden' }}>
        <Box component="ol" sx={{ m: 0, p: 0, listStyle: 'none' }}>
          {questions.map((item, i) => (
            <Box component="li" key={item.id} sx={{ '& + &': { borderTop: 1, borderColor: 'divider' } }}>
              <Button fullWidth color="inherit" onClick={() => setIndex(i)} aria-current={i === index ? 'step' : undefined}
                sx={{ justifyContent: 'flex-start', gap: 2, minHeight: 56, px: 2.5, borderRadius: 0, textAlign: 'left', fontWeight: i === index ? 600 : 400, bgcolor: i === index ? 'secondary.light' : 'transparent' }}>
                <Box component="span" sx={{ flex: '0 0 1.5ch' }}>{i + 1}</Box>
                {item.title}
              </Button>
            </Box>
          ))}
        </Box>
      </Paper>
    </>
  );
}

export default PatientInterview;
