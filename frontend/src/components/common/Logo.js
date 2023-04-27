import { Box } from '@mui/material';
import logo from '../../assets/logo.svg';

function Logo({ size = 32 }) {
  return <Box component="img" src={logo} alt="MediCase" sx={{ width: size, height: size, display: 'block' }} />;
}

export default Logo;
