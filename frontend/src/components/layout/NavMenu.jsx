import { useState } from 'react';
import { NavLink } from 'react-router';
import { IconButton, Menu, MenuItem } from '@mui/material';
import MenuIcon from '@mui/icons-material/Menu';

function NavMenu({ links }) {
  const [anchorEl, setAnchorEl] = useState(null);

  return (
    <>
      <IconButton color="inherit" aria-label="Menu" onClick={(e) => setAnchorEl(e.currentTarget)}>
        <MenuIcon />
      </IconButton>
      <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={() => setAnchorEl(null)}>
        {links.map((link) => (
          <MenuItem key={link.to} component={NavLink} to={link.to} onClick={() => setAnchorEl(null)}>
            {link.label}
          </MenuItem>
        ))}
      </Menu>
    </>
  );
}

export default NavMenu;
