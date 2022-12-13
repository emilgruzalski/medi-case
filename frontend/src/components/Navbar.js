import { Link } from 'react-router-dom';

function Navbar() {
  return (
    <nav>
      <Link to="/">Premedical</Link> | <Link to="/login">Zaloguj</Link> | <Link to="/register">Załóż konto</Link>
    </nav>
  );
}

export default Navbar;
