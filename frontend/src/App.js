import { Route, Routes } from 'react-router-dom';
import './App.css';
import Layout from './components/layout/Layout';
import Home from './components/landing/Home';
import Login from './components/user/Login';
import Register from './components/user/Register';
import SetLanguage from './components/user/SetLanguage';
import Learning from './components/learning/Learning';
import Moderator from './components/moderator/Moderator';
import Profile from './components/user/Profile';

function App() {
  return (
    <Routes>
      <Route element={<Layout />}>
        <Route index element={<Home />} />
        <Route path="login" element={<Login />} />
        <Route path="register" element={<Register />} />
        <Route path="language" element={<SetLanguage />} />
        <Route path="learning" element={<Learning />} />
        <Route path="learning/:entityId" element={<Learning />} />
        <Route path="moderator" element={<Moderator />} />
        <Route path="profile" element={<Profile />} />
      </Route>
    </Routes>
  );
}

export default App;
