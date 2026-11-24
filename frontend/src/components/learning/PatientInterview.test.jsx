import { fireEvent, render, screen } from '@testing-library/react';
import PatientInterview from './PatientInterview';
import useEntityInLanguages from '../../lib/useEntityInLanguages';
import { EntityTypes } from '../../lib/utility';
import { content } from '../../test/factories';

vi.mock('../../lib/useEntityInLanguages');

const interview = (titles) =>
  content('list', 'Rozmowa', EntityTypes.QuestionList, titles.map((title, i) => content(`q${i}`, title, EntityTypes.Question)));

const heading = () => screen.getByRole('heading', { level: 2 });

describe('PatientInterview', () => {
  beforeEach(() => {
    useEntityInLanguages.mockReturnValue({
      learning: interview(['What happened?', 'Where does it hurt?']),
      native: interview(['Co się stało?', 'Gdzie boli?']),
      error: '',
    });
  });

  it('shows one question at a time and stops at both ends', () => {
    render(<PatientInterview node={{ id: 'list' }} />);

    expect(heading()).toHaveTextContent('What happened?');
    expect(screen.getByLabelText('Poprzednie pytanie')).toBeDisabled();

    fireEvent.click(screen.getByLabelText('Następne pytanie'));

    expect(heading()).toHaveTextContent('Where does it hurt?');
    expect(screen.getByText('Pytanie 2 z 2')).toBeInTheDocument();
    expect(screen.getByLabelText('Następne pytanie')).toBeDisabled();
  });

  it('opens a question chosen from the list', () => {
    render(<PatientInterview node={{ id: 'list' }} />);

    fireEvent.click(screen.getByRole('button', { name: /Where does it hurt/ }));

    expect(heading()).toHaveTextContent('Where does it hurt?');
  });

  it('keeps the translation hidden until it is asked for', () => {
    render(<PatientInterview node={{ id: 'list' }} />);
    expect(screen.queryByText('Co się stało?')).not.toBeInTheDocument();

    fireEvent.click(screen.getByText('Pokaż tłumaczenie'));

    expect(screen.getByText('Co się stało?')).toBeInTheDocument();
  });

  it('explains that there are no questions in the language', () => {
    useEntityInLanguages.mockReturnValue({ learning: interview([]), native: null, error: '' });
    render(<PatientInterview node={{ id: 'list' }} />);

    expect(screen.getByText(/nie ma jeszcze pytań/)).toBeInTheDocument();
  });
});
