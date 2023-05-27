const DEFAULT_MESSAGE = 'Coś poszło nie tak. Spróbuj ponownie.';
const HTTP_BAD_GATEWAY = 502;
const AI_UNAVAILABLE_MESSAGE =
  'Usługa AI jest chwilowo niedostępna. Spróbuj ponownie później albo uzupełnij treść ręcznie.';

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

export function getAiErrorMessage(error) {
  return error?.response?.status === HTTP_BAD_GATEWAY ? AI_UNAVAILABLE_MESSAGE : getErrorMessage(error);
}
