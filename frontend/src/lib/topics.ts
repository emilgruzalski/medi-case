import { TreeNode } from '../network/types';
import { EntityTypes, typeLabel } from './utility';

const plural = new Intl.PluralRules('pl');
const QUESTION_FORMS: Record<string, string> = { one: 'pytanie', few: 'pytania' };

export const questionCount = (count: number) => `${count} ${QUESTION_FORMS[plural.select(count)] ?? 'pytań'}`;

/** Short description of a topic shown next to its title. */
export function topicCaption(node: TreeNode) {
  switch (node.typeId) {
    case EntityTypes.QuestionList:
    case EntityTypes.Test:
      return questionCount(node.children.length);
    case EntityTypes.Content:
      return 'Karta';
    default:
      return typeLabel(node.typeId);
  }
}

/** All categories of the tree, parents before their children. */
export const collectCategories = (nodes: TreeNode[]): TreeNode[] =>
  nodes
    .filter((node) => node.typeId === EntityTypes.Category)
    .flatMap((node) => [node, ...collectCategories(node.children)]);

/** Topics of a category without its subcategories, which are listed as separate sections. */
export const ownTopics = (category: TreeNode) =>
  category.children.filter((node) => node.typeId !== EntityTypes.Category);
