import { createTheme } from '@mui/material/styles';

const NAVY = '#14365c';
const YELLOW = '#ffc72c';
const INK = '#0f1e2e';

// Comfortable to tap with a thumb, also in gloves.
const TOUCH_TARGET = 44;

const theme = createTheme({
  palette: {
    primary: { main: NAVY, dark: '#0e2847', light: '#44648a', contrastText: '#ffffff' },
    secondary: { main: YELLOW, dark: '#e0aa0c', light: '#ffe08a', contrastText: INK },
    success: { main: '#0b5a3c' },
    error: { main: '#9b1c1c' },
    warning: { main: '#8a5a00' },
    text: { primary: INK, secondary: '#4b5d70' },
    background: { default: '#eef2f6', paper: '#ffffff' },
    divider: '#d5dde6',
  },
  shape: { borderRadius: 12 },
  typography: {
    fontFamily: ['"Lexend Variable"', 'Helvetica', 'Arial', 'sans-serif'].join(','),
    h1: { fontWeight: 600, letterSpacing: '-0.02em' },
    h2: { fontWeight: 600, letterSpacing: '-0.02em' },
    h3: { fontWeight: 600, letterSpacing: '-0.015em' },
    h4: { fontWeight: 600, letterSpacing: '-0.01em', fontSize: '1.75rem' },
    h5: { fontWeight: 600, letterSpacing: '-0.01em', fontSize: '1.4rem' },
    h6: { fontWeight: 600, fontSize: '1.125rem' },
    button: { fontWeight: 600, textTransform: 'none' },
  },
  components: {
    MuiPaper: { defaultProps: { elevation: 0 } },
    MuiAppBar: { defaultProps: { elevation: 0 } },
    MuiButton: {
      defaultProps: { disableElevation: true },
      styleOverrides: {
        root: { minHeight: TOUCH_TARGET, borderRadius: 12 },
        sizeSmall: { minHeight: 36 },
        sizeLarge: { minHeight: 56, paddingInline: 28, fontSize: '1.05rem' },
      },
    },
    MuiOutlinedInput: { styleOverrides: { root: { backgroundColor: '#ffffff' } } },
    MuiTab: { styleOverrides: { root: { textTransform: 'none', minHeight: 56 } } },
    MuiTabs: { styleOverrides: { indicator: { height: 4, backgroundColor: YELLOW } } },
    MuiTableCell: { styleOverrides: { head: { color: '#4b5d70', fontWeight: 500 } } },
    MuiListItemButton: { styleOverrides: { root: { minHeight: TOUCH_TARGET } } },
    MuiChip: { styleOverrides: { root: { fontWeight: 500 } } },
  },
});

export default theme;
