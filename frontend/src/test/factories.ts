import { EntityContent, TreeNode } from '../network/types';
import { EntityTypes } from '../lib/utility';

export const treeNode = (id: string, children: TreeNode[] = [], typeId: number = EntityTypes.Category): TreeNode => ({
  id,
  typeId,
  order: 0,
  isBlocked: false,
  title: `Title ${id}`,
  hasTranslation: true,
  children,
});

export const content = (
  id: string,
  title: string,
  typeId: number,
  children: EntityContent[] = [],
  isCorrect = false
): EntityContent => ({
  id,
  typeId,
  order: 0,
  isCorrect,
  langId: 1,
  translationId: `t-${id}`,
  title,
  content: null,
  hasTranslation: true,
  files: [],
  children,
});

export const sampleTest = () =>
  content('test', 'Test: badanie pacjenta', EntityTypes.Test, [
    content('q1', 'Co oznacza litera A?', EntityTypes.TestQuestion, [
      content('a1', 'Drogi oddechowe', EntityTypes.TestAnswer, [], true),
      content('a2', 'Alergie', EntityTypes.TestAnswer),
    ]),
    content('q2', 'Co oznacza litera S?', EntityTypes.TestQuestion, [
      content('a3', 'Saturacja', EntityTypes.TestAnswer),
      content('a4', 'Objawy', EntityTypes.TestAnswer, [], true),
    ]),
  ]);
