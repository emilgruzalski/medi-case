import { EntityContent } from '../network/types';

export type TestAnswers = Record<string, string>;

export interface TestScore {
  correct: number;
  total: number;
  percent: number;
}

export const isAnswerCorrect = (question: EntityContent, answerId: string | undefined) =>
  question.children.some((answer) => answer.id === answerId && answer.isCorrect);

export function calculateScore(questions: EntityContent[], answers: TestAnswers): TestScore {
  const correct = questions.filter((question) => isAnswerCorrect(question, answers[question.id])).length;
  const total = questions.length;
  return { correct, total, percent: total === 0 ? 0 : Math.round((correct / total) * 100) };
}
