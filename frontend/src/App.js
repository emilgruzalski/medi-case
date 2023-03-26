import { Route, Routes } from 'react-router-dom';
import './App.css';
import Layout from './components/Layout';
import Home from './components/Home';
import Login from './components/Login';
import Register from './components/Register';
import SetLanguage from './components/SetLanguage';
import Learning from './components/Learning';
import Moderator from './components/Moderator';
import Profile from './components/Profile';

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
