const DEFAULT_MESSAGE = 'Coś poszło nie tak. Spróbuj ponownie.';

export function getErrorMessage(error, fallback = DEFAULT_MESSAGE) {
  const data = error?.response?.data;
  if (data?.message) {
    return data.message;
  }
  if (data?.errors) {
    return Object.values(data.errors).flat()[0] ?? fallback;
  }
  if (error?.request && !error.response) {
    return 'Brak połączenia z serwerem.';
  }
  return fallback;
}
