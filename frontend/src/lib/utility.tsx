import { ReactElement } from 'react';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import FolderOutlinedIcon from '@mui/icons-material/FolderOutlined';
import ForumOutlinedIcon from '@mui/icons-material/ForumOutlined';
import HelpOutlineIcon from '@mui/icons-material/HelpOutline';
import QuizOutlinedIcon from '@mui/icons-material/QuizOutlined';
import { TreeNode } from '../network/types';

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

const TYPE_ICONS: Record<number, ReactElement> = {
  [EntityTypes.Category]: <FolderOutlinedIcon fontSize="small" />,
  [EntityTypes.Content]: <ArticleOutlinedIcon fontSize="small" />,
  [EntityTypes.QuestionList]: <ForumOutlinedIcon fontSize="small" />,
  [EntityTypes.Question]: <ChatBubbleOutlineIcon fontSize="small" />,
  [EntityTypes.Test]: <QuizOutlinedIcon fontSize="small" />,
  [EntityTypes.TestQuestion]: <HelpOutlineIcon fontSize="small" />,
  [EntityTypes.TestAnswer]: <CheckCircleOutlineIcon fontSize="small" />,
};

export const typeLabel = (typeId: number) => TYPE_LABELS[typeId] ?? 'Element';

export const typeIcon = (typeId: number) => TYPE_ICONS[typeId] ?? <ArticleOutlinedIcon fontSize="small" />;

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
