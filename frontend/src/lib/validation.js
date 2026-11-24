export const MIN_PASSWORD_LENGTH = 8;

// Same rules as the password policy of the Cognito user pool.
export const PASSWORD_RULES_MESSAGE =
  `Hasło musi mieć co najmniej ${MIN_PASSWORD_LENGTH} znaków, małą i wielką literę, cyfrę oraz znak specjalny.`;

const PASSWORD_PATTERNS = [/[a-z]/, /[A-Z]/, /\d/, /[^A-Za-z0-9]/];
const EMAIL_PATTERN = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

export function isValidEmail(email) {
  return EMAIL_PATTERN.test(email.trim());
}

export function isStrongPassword(password) {
  return password.length >= MIN_PASSWORD_LENGTH && PASSWORD_PATTERNS.every((pattern) => pattern.test(password));
}

export function validatePassword(password, confirmPassword) {
  if (!isStrongPassword(password)) {
    return PASSWORD_RULES_MESSAGE;
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
