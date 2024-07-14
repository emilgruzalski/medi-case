import { getAiErrorMessage, getErrorMessage } from './errors';
import { CognitoError } from '../network/cognito';

describe('getErrorMessage', () => {
  it('uses the API message', () => {
    expect(getErrorMessage({ response: { data: { message: 'Brak dostępu' } } })).toBe('Brak dostępu');
  });

  it('uses the first validation error', () => {
    expect(getErrorMessage({ response: { data: { errors: { Email: ['Niepoprawny e-mail'] } } } })).toBe('Niepoprawny e-mail');
  });

  it('reports missing connection', () => {
    expect(getErrorMessage({ request: {} })).toBe('Brak połączenia z serwerem.');
  });

  it('translates Cognito errors', () => {
    expect(getErrorMessage(new CognitoError('NotAuthorizedException', 'Incorrect username or password.')))
      .toBe('Nieprawidłowy e-mail lub hasło.');
  });

  it('explains when the AI service is unavailable', () => {
    expect(getAiErrorMessage({ response: { status: 502, data: { message: 'AWS' } } })).toMatch(/Usługa AI/);
  });
});
