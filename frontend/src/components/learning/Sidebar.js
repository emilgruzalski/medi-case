import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Collapse, List, ListItemButton, ListItemIcon, ListItemText, Paper } from '@mui/material';
import ExpandLess from '@mui/icons-material/ExpandLess';
import ExpandMore from '@mui/icons-material/ExpandMore';
import { typeIcon } from '../../lib/utility';

const INDENT_STEP = 2;

function SidebarItem({ node, depth, selectedId }) {
  const [open, setOpen] = useState(true);
  const navigate = useNavigate();
  const hasChildren = node.children.length > 0;

  return (
    <>
      <ListItemButton
        selected={node.id === selectedId}
        sx={{ pl: 2 + depth * INDENT_STEP }}
        onClick={() => navigate(`/learning/${node.id}`)}
      >
        <ListItemIcon sx={{ minWidth: 32 }}>{typeIcon(node.typeId)}</ListItemIcon>
        <ListItemText primary={node.title} />
        {hasChildren && (
          <span
            onClick={(event) => {
              event.stopPropagation();
              setOpen(!open);
            }}
          >
            {open ? <ExpandLess /> : <ExpandMore />}
          </span>
        )}
      </ListItemButton>
      {hasChildren && (
        <Collapse in={open} timeout="auto" unmountOnExit>
          <List disablePadding>
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
    <Paper>
      <List dense>
        {tree.map((node) => (
          <SidebarItem key={node.id} node={node} depth={0} selectedId={selectedId} />
        ))}
      </List>
    </Paper>
  );
}

export default Sidebar;
