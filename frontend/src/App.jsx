import { lazy, Suspense } from 'react';
import { Route, Routes } from 'react-router-dom';
import './App.css';
import Layout from './components/layout/Layout';
import ProtectedRoute from './components/common/ProtectedRoute';
import Home from './components/landing/Home';
import Login from './components/user/Login';
import Register from './components/user/Register';
import ConfirmAccount from './components/user/ConfirmAccount';
import ForgotPassword from './components/user/ForgotPassword';
import SetLanguage from './components/user/SetLanguage';
import Profile from './components/user/Profile';
import Learning from './components/learning/Learning';
import PageLoader from './components/common/PageLoader';
import { canModerate, isAdmin } from './lib/roles';

// Moderator and admin pages (with the rich text editor) are loaded only for users who open them.
const Moderator = lazy(() => import('./components/moderator/Moderator'));
const BoardAdmin = lazy(() => import('./components/admin/BoardAdmin'));

function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="confirm" element={<ConfirmAccount />} />
        <Route path="forgot-password" element={<ForgotPassword />} />
        <Route element={<ProtectedRoute />}>
          <Route path="language" element={<SetLanguage />} />
          <Route path="learning" element={<Learning />} />
          <Route path="learning/:entityId" element={<Learning />} />
          <Route path="profile" element={<Profile />} />
        </Route>
        <Route element={<ProtectedRoute allow={canModerate} />}>
          <Route path="moderator" element={<Suspense fallback={<PageLoader />}><Moderator /></Suspense>} />
        </Route>
        <Route element={<ProtectedRoute allow={isAdmin} />}>
          <Route path="admin" element={<Suspense fallback={<PageLoader />}><BoardAdmin /></Suspense>} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
