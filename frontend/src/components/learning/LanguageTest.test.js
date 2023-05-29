import { fireEvent, render, screen } from '@testing-library/react';
import LanguageTest from './LanguageTest';
import { sampleTest } from '../../test/factories';

describe('LanguageTest', () => {
  it('shows the score after all questions are answered and allows a retry', () => {
    render(<LanguageTest test={sampleTest()} />);

    fireEvent.click(screen.getByText('Drogi oddechowe'));
    expect(screen.getByText('Odpowiedzi: 1 / 2')).toBeInTheDocument();

    fireEvent.click(screen.getByText('Saturacja'));
    expect(screen.getByText('Twój wynik: 1 / 2 (50%)')).toBeInTheDocument();

    fireEvent.click(screen.getByText('Spróbuj ponownie'));
    expect(screen.getByText('Odpowiedzi: 0 / 2')).toBeInTheDocument();
    expect(screen.queryByText(/Twój wynik/)).not.toBeInTheDocument();
  });

  it('does not change an answer that was already given', () => {
    render(<LanguageTest test={sampleTest()} />);

    fireEvent.click(screen.getByText('Alergie'));
    fireEvent.click(screen.getByText('Drogi oddechowe'));

    expect(screen.getByText('Odpowiedzi: 1 / 2')).toBeInTheDocument();
  });
});
