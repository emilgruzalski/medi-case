import { Outlet } from 'react-router';
import Navbar from './Navbar';
import Footer from './Footer';

function Layout() {
  return (
    <div className="app">
      <Navbar />
      <main className="app-content">
        <Outlet />
      </main>
      <Footer />
    </div>
  );
}

export default Layout;
