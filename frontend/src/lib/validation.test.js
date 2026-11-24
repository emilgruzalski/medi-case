import { isStrongPassword, isValidEmail, validatePassword, validateRegistration } from './validation';

const form = {
  firstName: 'Jan', lastName: 'Kowalski', email: 'jan@example.com', password: 'Tajne-haslo1', confirmPassword: 'Tajne-haslo1',
};

describe('validation', () => {
  it('accepts a correct registration form', () => {
    expect(validateRegistration(form)).toBe('');
  });

  it('requires the password policy of the user pool', () => {
    expect(isStrongPassword('tajnehaslo')).toBe(false);
    expect(isStrongPassword('Tajnehaslo1')).toBe(false);
    expect(isStrongPassword('Tajne-haslo1')).toBe(true);
  });

  it('rejects different passwords', () => {
    expect(validatePassword('Tajne-haslo1', 'Inne-haslo1')).toBe('Hasła nie są takie same.');
  });

  it('checks e-mail addresses', () => {
    expect(isValidEmail('jan@example.com')).toBe(true);
    expect(isValidEmail('jan@')).toBe(false);
  });
});
