import { useEffect, useState } from 'react';
import useStore from '../state/store';
import { getLearningTree } from '../network/lib/learning';
import { getErrorMessage } from './errors';

/** Published content tree in the learning language. */
export default function useLearningTree() {
  const learningLangId = useStore((state) => state.learningLangId);
  const [state, setState] = useState({ tree: null, error: '' });

  useEffect(() => {
    let active = true;
    if (!learningLangId) {
      return undefined;
    }
    setState({ tree: null, error: '' });
    getLearningTree(learningLangId)
      .then((tree) => active && setState({ tree, error: '' }))
      .catch((err) => active && setState({ tree: null, error: getErrorMessage(err) }));
    return () => {
      active = false;
    };
  }, [learningLangId]);

  return state;
}
