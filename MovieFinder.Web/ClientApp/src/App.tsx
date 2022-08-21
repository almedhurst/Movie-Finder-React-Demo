import {useCallback, useEffect} from 'react';
import './App.css';
import {useAppDispatch, useAppSelector} from "./core/store/configureStore";
import {loadingSaveState} from "./core/slices/siteAppearanceSlice";
import {Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import HeaderComponent from "./shared/HeaderComponent";
import {Route, Routes} from 'react-router-dom';
import {ToastContainer} from "react-toastify";
import ServerError from "./features/errors/ServerError";
import NotFound from "./features/errors/NotFound";
import MovieHome from "./features/MovieListings/MovieHome";
import MovieDetails from './features/MovieListings/MovieDetails';
import MovieSearch from "./features/MovieListings/MovieSearch";
import Category from "./features/MovieListings/Category";

function App() {
    const dispatch = useAppDispatch();
    const {darkMode} = useAppSelector(state => state.siteAppearance);
    const initApp = useCallback(async () => {
        dispatch(loadingSaveState());
    }, [dispatch]);

    useEffect(() => {
        initApp();
    }, [initApp])

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
            <ToastContainer position='top-center' hideProgressBar theme='colored'/>
            <CssBaseline/>
            <HeaderComponent/>
            <Container>
                <Routes>
                    <Route path='/' element={<MovieHome/>}/>
                    <Route path='/movie/:name/:id' element={<MovieDetails />}/>
                    <Route path='/category/:name/:id' element={<Category />}/>
                    <Route path='/search' element={<MovieSearch />} />
                    <Route path='/server-error' element={<ServerError/>}/>
                    <Route path='*' element={<NotFound/>}/>
                </Routes>
            </Container>
        </ThemeProvider>
    );
}

export default App;
