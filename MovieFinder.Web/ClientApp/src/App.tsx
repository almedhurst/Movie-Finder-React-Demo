import {useCallback, useEffect} from 'react';
import './App.css';
import {useAppDispatch, useAppSelector} from "./core/store/configureStore";
import {loadingSaveState} from "./core/slices/siteAppearanceSlice";
import {Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import HeaderComponent from "./shared/HeaderComponent";
import HomePage from "./features/Home/HomePage";
import { Route, Routes } from 'react-router-dom';

function App() {
  const dispatch = useAppDispatch();
  const {darkMode} = useAppSelector(state => state.siteAppearance);
  const initApp = useCallback(async () => {
    dispatch(loadingSaveState());
  }, [dispatch]);
  
  useEffect(() => {
      initApp();
  },[initApp])

  const paletteType = darkMode ? 'dark' : 'light';
  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: {
        default: paletteType === 'light' ? '#eaeaea' : '#121212'
      }
    }
  })
  
  return (
      <ThemeProvider theme={theme}>
        <CssBaseline />
        <HeaderComponent />
        <Container>
            <Routes>
                <Route path='/' element={<HomePage />} />
            </Routes>
        </Container>
      </ThemeProvider>
  );
}

export default App;
