import { EntityType, Translation, TreeNode } from '../network/types';

export const EntityTypes = {
  Category: 1,
  Content: 2,
  QuestionList: 3,
  Question: 4,
  Test: 5,
  TestQuestion: 6,
  TestAnswer: 7,
} as const;

const TYPE_LABELS: Record<number, string> = {
  [EntityTypes.Category]: 'Dział',
  [EntityTypes.Content]: 'Karta z treścią',
  [EntityTypes.QuestionList]: 'Rozmowa z pacjentem',
  [EntityTypes.Question]: 'Pytanie',
  [EntityTypes.Test]: 'Test',
  [EntityTypes.TestQuestion]: 'Pytanie testowe',
  [EntityTypes.TestAnswer]: 'Odpowiedź',
};

export const typeLabel = (typeId: number) => TYPE_LABELS[typeId] ?? 'Element';

export const findTranslation = (translations: Translation[], langId: number | null) =>
  translations.find((t) => t.langId === langId);

const BYTES_IN_KILOBYTE = 1024;

export const formatFileSize = (bytes: number) =>
  bytes < BYTES_IN_KILOBYTE * BYTES_IN_KILOBYTE
    ? `${Math.ceil(bytes / BYTES_IN_KILOBYTE)} KB`
    : `${(bytes / (BYTES_IN_KILOBYTE * BYTES_IN_KILOBYTE)).toFixed(1)} MB`;

export const isBlankHtml = (html: string | null | undefined) =>
  !html || html.replace(/<[^>]*>/g, '').replace(/&nbsp;/g, ' ').trim() === '';

export function findNode(nodes: TreeNode[], id: string | undefined): TreeNode | undefined {
  for (const node of nodes) {
    if (node.id === id) {
      return node;
    }
    const found = findNode(node.children, id);
    if (found) {
      return found;
    }
  }
  return undefined;
}

export function findSiblings(nodes: TreeNode[], id: string | undefined): TreeNode[] | undefined {
  if (nodes.some((node) => node.id === id)) {
    return nodes;
  }
  for (const node of nodes) {
    const siblings = findSiblings(node.children, id);
    if (siblings) {
      return siblings;
    }
  }
  return undefined;
}

export const allowedChildTypes = (types: EntityType[], parentTypeId: number | null) =>
  parentTypeId === null
    ? types.filter((type) => type.canBeRoot).map((type) => type.id)
    : types.find((type) => type.id === parentTypeId)?.allowedChildTypeIds ?? [];
