import { useEffect, useState } from 'react';

export const SEARCH_DELAY_MS = 300;

export default function useDebouncedValue(value, delay = SEARCH_DELAY_MS) {
  const [debounced, setDebounced] = useState(value);

  useEffect(() => {
    const timeout = setTimeout(() => setDebounced(value), delay);
    return () => clearTimeout(timeout);
  }, [value, delay]);

  return debounced;
}
