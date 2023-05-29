import { getAiErrorMessage, getErrorMessage } from './errors';

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

  it('explains when the AI service is unavailable', () => {
    expect(getAiErrorMessage({ response: { status: 502, data: { message: 'AWS' } } })).toMatch(/Usługa AI/);
  });
});
