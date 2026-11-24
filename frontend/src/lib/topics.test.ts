import { collectCategories, ownTopics, questionCount, topicCaption } from './topics';
import { EntityTypes } from './utility';
import { treeNode } from '../test/factories';

describe('questionCount', () => {
  it('uses Polish plural forms', () => {
    expect([1, 2, 4, 5, 12, 22, 25].map(questionCount)).toEqual([
      '1 pytanie', '2 pytania', '4 pytania', '5 pytań', '12 pytań', '22 pytania', '25 pytań',
    ]);
  });
});

describe('topicCaption', () => {
  it('counts questions of interviews and tests', () => {
    const questions = [treeNode('q1', [], EntityTypes.Question), treeNode('q2', [], EntityTypes.Question)];

    expect(topicCaption(treeNode('list', questions, EntityTypes.QuestionList))).toBe('2 pytania');
    expect(topicCaption(treeNode('card', [], EntityTypes.Content))).toBe('Karta');
  });
});

describe('collectCategories', () => {
  const card = treeNode('card', [], EntityTypes.Content);
  const tree = [treeNode('a', [card, treeNode('a1', [treeNode('a11')])]), treeNode('b')];

  it('lists nested categories after their parents', () => {
    expect(collectCategories(tree).map((node) => node.id)).toEqual(['a', 'a1', 'a11', 'b']);
  });

  it('leaves subcategories out of the topics of a category', () => {
    expect(ownTopics(tree[0]).map((node) => node.id)).toEqual(['card']);
  });
});
