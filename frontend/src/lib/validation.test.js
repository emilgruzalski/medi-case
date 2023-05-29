import { isValidEmail, validatePassword, validateRegistration } from './validation';

const form = { firstName: 'Jan', lastName: 'Kowalski', email: 'jan@example.com', password: 'tajnehaslo', confirmPassword: 'tajnehaslo' };

describe('validation', () => {
  it('accepts a correct registration form', () => {
    expect(validateRegistration(form)).toBe('');
  });

  it('rejects short or different passwords', () => {
    expect(validatePassword('krotkie', 'krotkie')).not.toBe('');
    expect(validatePassword('tajnehaslo', 'innehaslo')).toBe('Hasła nie są takie same.');
  });

  it('checks e-mail addresses', () => {
    expect(isValidEmail('jan@example.com')).toBe(true);
    expect(isValidEmail('jan@')).toBe(false);
  });
});
