import { PASSWORD_RULES_MESSAGE } from './validation';

const DEFAULT_MESSAGE = 'Coś poszło nie tak. Spróbuj ponownie.';
const NO_CONNECTION_MESSAGE = 'Brak połączenia z serwerem.';
const HTTP_BAD_GATEWAY = 502;
const AI_UNAVAILABLE_MESSAGE =
  'Usługa AI jest chwilowo niedostępna. Spróbuj ponownie później albo uzupełnij treść ręcznie.';

const TOO_MANY_ATTEMPTS = 'Zbyt wiele prób. Spróbuj ponownie za chwilę.';
const WRONG_CREDENTIALS = 'Nieprawidłowy e-mail lub hasło.';

const COGNITO_MESSAGES = {
  NotAuthorizedException: WRONG_CREDENTIALS,
  UserNotFoundException: WRONG_CREDENTIALS,
  UserNotConfirmedException: 'Konto nie zostało jeszcze potwierdzone.',
  UsernameExistsException: 'Konto z tym adresem e-mail już istnieje.',
  InvalidPasswordException: PASSWORD_RULES_MESSAGE,
  CodeMismatchException: 'Nieprawidłowy kod.',
  ExpiredCodeException: 'Kod wygasł. Poproś o nowy.',
  LimitExceededException: TOO_MANY_ATTEMPTS,
  TooManyRequestsException: TOO_MANY_ATTEMPTS,
};

export function getErrorMessage(error, fallback = DEFAULT_MESSAGE) {
  if (error?.name === 'CognitoError') {
    return COGNITO_MESSAGES[error.code] ?? fallback;
  }
  if (error instanceof TypeError) {
    // fetch() rejects with a TypeError when the server cannot be reached.
    return NO_CONNECTION_MESSAGE;
  }
  const data = error?.response?.data;
  if (data?.message) {
    return data.message;
  }
  if (data?.errors) {
    return Object.values(data.errors).flat()[0] ?? fallback;
  }
  if (error?.request && !error.response) {
    return NO_CONNECTION_MESSAGE;
  }
  return fallback;
}

/** Wrong credentials are reported with one message, whatever the exact Cognito error is. */
export function getSignInErrorMessage(error) {
  return error?.code === 'InvalidPasswordException' ? WRONG_CREDENTIALS : getErrorMessage(error);
}

export function getAiErrorMessage(error) {
  return error?.response?.status === HTTP_BAD_GATEWAY ? AI_UNAVAILABLE_MESSAGE : getErrorMessage(error);
}
