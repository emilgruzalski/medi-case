import { FormControlLabel, Stack, Switch } from '@mui/material';
import { updateEntity } from '../../network/lib/entity';
import { EntityTypes } from '../../lib/utility';

function EntitySettings({ entity, onChanged, onError }) {
  const save = async (changes) => {
    try {
      await updateEntity(entity.id, {
        order: entity.order,
        isCorrect: entity.isCorrect,
        isBlocked: entity.isBlocked,
        ...changes,
      });
      onChanged();
    } catch (err) {
      onError(err);
    }
  };

  return (
    <Stack direction="row" spacing={2} sx={{ mb: 2 }}>
      <FormControlLabel
        control={<Switch checked={entity.isBlocked} onChange={(e) => save({ isBlocked: e.target.checked })} />}
        label="Ukryte dla uczniów"
      />
      {entity.typeId === EntityTypes.TestAnswer && (
        <FormControlLabel
          control={<Switch color="success" checked={entity.isCorrect}
            onChange={(e) => save({ isCorrect: e.target.checked })} />}
          label="Poprawna odpowiedź"
        />
      )}
    </Stack>
  );
}

export default EntitySettings;
