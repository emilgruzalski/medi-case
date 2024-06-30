import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Collapse, IconButton, List, ListItem, ListItemButton, ListItemIcon, ListItemText, Paper } from '@mui/material';
import ExpandLess from '@mui/icons-material/ExpandLess';
import ExpandMore from '@mui/icons-material/ExpandMore';
import TypeIcon from '../common/TypeIcon';

const INDENT_STEP = 2;
const SIDEBAR_MAX_HEIGHT = 'calc(100vh - 140px)';

function SidebarItem({ node, depth, selectedId }) {
  const [open, setOpen] = useState(depth === 0);
  const navigate = useNavigate();
  const hasChildren = node.children.length > 0;

  return (
    <>
      <ListItem
        disablePadding
        secondaryAction={
          hasChildren && (
            <IconButton edge="end" size="small" aria-label={open ? 'Zwiń' : 'Rozwiń'} onClick={() => setOpen(!open)}>
              {open ? <ExpandLess /> : <ExpandMore />}
            </IconButton>
          )
        }
      >
        <ListItemButton
          selected={node.id === selectedId}
          sx={{ pl: 2 + depth * INDENT_STEP }}
          onClick={() => navigate(`/learning/${node.id}`)}
        >
          <ListItemIcon sx={{ minWidth: 32 }}><TypeIcon typeId={node.typeId} /></ListItemIcon>
          <ListItemText primary={node.title} />
        </ListItemButton>
      </ListItem>
      {hasChildren && (
        <Collapse in={open} timeout="auto" unmountOnExit>
          <List disablePadding dense>
            {node.children.map((child) => (
              <SidebarItem key={child.id} node={child} depth={depth + 1} selectedId={selectedId} />
            ))}
          </List>
        </Collapse>
      )}
    </>
  );
}

function Sidebar({ tree, selectedId }) {
  return (
    <Paper sx={{ position: { md: 'sticky' }, top: { md: 88 }, maxHeight: { md: SIDEBAR_MAX_HEIGHT }, overflowY: 'auto' }}>
      <List dense>
        {tree.map((node) => (
          <SidebarItem key={node.id} node={node} depth={0} selectedId={selectedId} />
        ))}
      </List>
    </Paper>
  );
}

export default Sidebar;
