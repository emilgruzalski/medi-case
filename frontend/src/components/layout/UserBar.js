import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { Button, Divider, ListItemIcon, Menu, MenuItem, Typography } from '@mui/material';
import AccountCircleIcon from '@mui/icons-material/AccountCircle';
import PersonOutlineIcon from '@mui/icons-material/PersonOutline';
import LogoutIcon from '@mui/icons-material/Logout';
import useStore from '../../state/store';
import { logout } from '../../network/lib/session';

function UserBar() {
  const user = useStore((state) => state.user);
  const clearSession = useStore((state) => state.clearSession);
  const [anchorEl, setAnchorEl] = useState(null);
  const navigate = useNavigate();

  const handleLogout = async () => {
    setAnchorEl(null);
    try {
      await logout();
    } finally {
      clearSession();
      navigate('/login');
    }
  };

  return (
    <>
      <Button color="inherit" startIcon={<AccountCircleIcon />} onClick={(e) => setAnchorEl(e.currentTarget)}>
        {user.firstName} {user.lastName}
      </Button>
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={() => setAnchorEl(null)}>
        <Typography variant="body2" color="text.secondary" sx={{ px: 2, py: 1 }}>
          {user.email}
        </Typography>
        <Divider />
        <MenuItem
          onClick={() => {
            setAnchorEl(null);
            navigate('/profile');
          }}
        >
          <ListItemIcon>
            <PersonOutlineIcon fontSize="small" />
          </ListItemIcon>
          Profil
        </MenuItem>
        <MenuItem onClick={handleLogout}>
          <ListItemIcon>
            <LogoutIcon fontSize="small" />
          </ListItemIcon>
          Wyloguj
        </MenuItem>
      </Menu>
    </>
  );
}

export default UserBar;
