import { useEffect, useState } from 'react';
import useStore from '../state/store';
import { getEntityWithChildren } from '../network/lib/entity';
import { getErrorMessage } from './errors';

/**
 * Loads an entity with its children in the learning language and, when it is different, in the native language.
 */
export default function useEntityInLanguages(entityId) {
  const { learningLangId, nativeLangId } = useStore();
  const [state, setState] = useState({ learning: null, native: null, error: '' });

  useEffect(() => {
    let active = true;
    const needsNative = nativeLangId && nativeLangId !== learningLangId;
    setState({ learning: null, native: null, error: '' });

    Promise.all([
      getEntityWithChildren(entityId, learningLangId),
      needsNative ? getEntityWithChildren(entityId, nativeLangId) : Promise.resolve(null),
    ])
      .then(([learning, native]) => active && setState({ learning, native, error: '' }))
      .catch((err) => active && setState({ learning: null, native: null, error: getErrorMessage(err) }));

    return () => {
      active = false;
    };
  }, [entityId, learningLangId, nativeLangId]);

  return state;
}
