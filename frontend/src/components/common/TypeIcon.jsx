import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import ChatBubbleOutlineIcon from '@mui/icons-material/ChatBubbleOutline';
import CheckCircleOutlineIcon from '@mui/icons-material/CheckCircleOutline';
import FolderOutlinedIcon from '@mui/icons-material/FolderOutlined';
import ForumOutlinedIcon from '@mui/icons-material/ForumOutlined';
import HelpOutlineIcon from '@mui/icons-material/HelpOutline';
import QuizOutlinedIcon from '@mui/icons-material/QuizOutlined';
import { EntityTypes } from '../../lib/utility';

const ICONS = {
  [EntityTypes.Category]: FolderOutlinedIcon,
  [EntityTypes.Content]: ArticleOutlinedIcon,
  [EntityTypes.QuestionList]: ForumOutlinedIcon,
  [EntityTypes.Question]: ChatBubbleOutlineIcon,
  [EntityTypes.Test]: QuizOutlinedIcon,
  [EntityTypes.TestQuestion]: HelpOutlineIcon,
  [EntityTypes.TestAnswer]: CheckCircleOutlineIcon,
};

function TypeIcon({ typeId }) {
  const Icon = ICONS[typeId] ?? ArticleOutlinedIcon;
  return <Icon fontSize="small" />;
}

export default TypeIcon;
