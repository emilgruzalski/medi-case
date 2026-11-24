import { calculateScore, isAnswerCorrect } from './testScore';
import { sampleTest } from '../test/factories';

describe('calculateScore', () => {
  const questions = sampleTest().children;

  it('counts correct answers and percent', () => {
    expect(calculateScore(questions, { q1: 'a1', q2: 'a3' })).toEqual({ correct: 1, total: 2, percent: 50 });
  });

  it('returns zero for no questions', () => {
    expect(calculateScore([], {})).toEqual({ correct: 0, total: 0, percent: 0 });
  });

  it('checks a single answer', () => {
    expect(isAnswerCorrect(questions[1], 'a4')).toBe(true);
    expect(isAnswerCorrect(questions[1], undefined)).toBe(false);
  });
});
