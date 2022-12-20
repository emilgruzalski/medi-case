export const MIN_PASSWORD_LENGTH = 8;

const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export function isValidEmail(email) {
  return EMAIL_PATTERN.test(email.trim());
}

export function validatePassword(password, confirmPassword) {
  if (password.length < MIN_PASSWORD_LENGTH) {
    return `Hasło musi mieć co najmniej ${MIN_PASSWORD_LENGTH} znaków.`;
  }
  if (password !== confirmPassword) {
    return 'Hasła nie są takie same.';
  }
  return '';
}

export function validateRegistration(form) {
  if (!form.firstName.trim() || !form.lastName.trim()) {
    return 'Podaj imię i nazwisko.';
  }
  if (!isValidEmail(form.email)) {
    return 'Podaj poprawny adres e-mail.';
  }
  return validatePassword(form.password, form.confirmPassword);
}
