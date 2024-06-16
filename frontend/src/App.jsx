import { Route, Routes } from 'react-router-dom';
import './App.css';
import Layout from './components/layout/Layout';
import ProtectedRoute from './components/common/ProtectedRoute';
import Home from './components/landing/Home';
import Login from './components/user/Login';
import Register from './components/user/Register';
import SetLanguage from './components/user/SetLanguage';
import Profile from './components/user/Profile';
import Learning from './components/learning/Learning';
import Moderator from './components/moderator/Moderator';
import BoardAdmin from './components/admin/BoardAdmin';
import { canModerate, isAdmin } from './lib/roles';

function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route element={<ProtectedRoute />}>
          <Route path="language" element={<SetLanguage />} />
          <Route path="learning" element={<Learning />} />
          <Route path="learning/:entityId" element={<Learning />} />
          <Route path="profile" element={<Profile />} />
        </Route>
        <Route element={<ProtectedRoute allow={canModerate} />}>
          <Route path="moderator" element={<Moderator />} />
        </Route>
        <Route element={<ProtectedRoute allow={isAdmin} />}>
          <Route path="admin" element={<BoardAdmin />} />
        </Route>
      </Route>
    </Routes>
  );
}

export default App;
