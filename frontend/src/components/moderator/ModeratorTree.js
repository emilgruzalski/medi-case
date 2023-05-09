import { useState } from 'react';
import { Box, Button, IconButton, List, ListItemButton, ListItemIcon, ListItemText, Paper, Tooltip } from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import DeleteOutlineIcon from '@mui/icons-material/DeleteOutline';
import VisibilityOffOutlinedIcon from '@mui/icons-material/VisibilityOffOutlined';
import AddNodeDialog from './AddNodeDialog';
import ConfirmDialog from '../common/ConfirmDialog';
import { deleteEntity } from '../../network/lib/entity';
import { typeIcon } from '../../lib/utility';

const INDENT_STEP = 2;
const TREE_MAX_HEIGHT = 'calc(100vh - 260px)';

function TreeItem({ node, depth, selectedId, onSelect, onAdd, onDelete }) {
  return (
    <>
      <ListItemButton selected={node.id === selectedId} sx={{ pl: 1 + depth * INDENT_STEP }} onClick={() => onSelect(node)}>
        <ListItemIcon sx={{ minWidth: 32 }}>{typeIcon(node.typeId)}</ListItemIcon>
        <Tooltip title={node.hasTranslation ? '' : 'Brak tłumaczenia w wybranym języku'}>
          <ListItemText
            primary={node.title}
            primaryTypographyProps={{ color: node.hasTranslation ? 'text.primary' : 'text.disabled', noWrap: true }}
          />
        </Tooltip>
        {node.isBlocked && (
          <Tooltip title="Ukryte dla uczniów">
            <VisibilityOffOutlinedIcon fontSize="small" color="action" sx={{ mx: 0.5 }} />
          </Tooltip>
        )}
        <Tooltip title="Dodaj element">
          <IconButton size="small" onClick={(e) => { e.stopPropagation(); onAdd(node); }}>
            <AddIcon fontSize="small" />
          </IconButton>
        </Tooltip>
        <Tooltip title="Usuń">
          <IconButton size="small" onClick={(e) => { e.stopPropagation(); onDelete(node); }}>
            <DeleteOutlineIcon fontSize="small" />
          </IconButton>
        </Tooltip>
      </ListItemButton>
      {node.children.map((child) => (
        <TreeItem key={child.id} node={child} depth={depth + 1} selectedId={selectedId}
          onSelect={onSelect} onAdd={onAdd} onDelete={onDelete} />
      ))}
    </>
  );
}

function ModeratorTree({ tree, langId, selectedId, onSelect, onChanged, onDeleted }) {
  const [addParent, setAddParent] = useState(undefined);
  const [toDelete, setToDelete] = useState(null);

  const handleDelete = async () => {
    await deleteEntity(toDelete.id);
    setToDelete(null);
    onDeleted(toDelete.id);
  };

  return (
    <Paper>
      <Box sx={{ p: 1 }}>
        <Button startIcon={<AddIcon />} onClick={() => setAddParent(null)}>
          Nowy dział
        </Button>
      </Box>
      <List dense sx={{ maxHeight: TREE_MAX_HEIGHT, overflowY: 'auto' }}>
        {tree.map((node) => (
          <TreeItem key={node.id} node={node} depth={0} selectedId={selectedId}
            onSelect={onSelect} onAdd={setAddParent} onDelete={setToDelete} />
        ))}
      </List>
      <AddNodeDialog
        open={addParent !== undefined}
        parent={addParent}
        langId={langId}
        onClose={() => setAddParent(undefined)}
        onCreated={(entity) => {
          setAddParent(undefined);
          onChanged(entity.id);
        }}
      />
      <ConfirmDialog
        open={Boolean(toDelete)}
        title="Usunąć element?"
        message={`Element „${toDelete?.title}” zostanie usunięty razem ze wszystkimi elementami w środku.`}
        onConfirm={handleDelete}
        onClose={() => setToDelete(null)}
      />
    </Paper>
  );
}

export default ModeratorTree;
