import {useCallback, useEffect, useState} from 'react';
import './App.css';
import {useAppDispatch, useAppSelector} from "./core/store/configureStore";
import {loadingSaveState} from "./core/slices/siteAppearanceSlice";
import {Container, createTheme, CssBaseline, ThemeProvider} from "@mui/material";
import HeaderComponent from "./shared/HeaderComponent";
import {Route, Routes} from 'react-router-dom';
import {ToastContainer} from "react-toastify";
import ServerError from "./features/errors/ServerError";
import NotFound from "./features/errors/NotFound";
import MovieHome from "./features/movielistings/MovieHome";
import MovieDetails from './features/movielistings/MovieDetails';
import MovieSearch from "./features/movielistings/MovieSearch";
import Category from "./features/movielistings/Category";
import Login from "./features/account/Login";
import {fetchCurrentUserAsync} from "./core/slices/accountSlice";
import 'react-toastify/dist/ReactToastify.css';
import PrivateComponent from "./shared/PrivateComponent";
import FavouriteMovies from "./features/account/FavouriteMovies";
import LoadingComponent from "./shared/LoadingComponent";
import Register from "./features/account/Register";

function App() {
    const dispatch = useAppDispatch();
    const [loading, setLoading] = useState(true);
    const {darkMode} = useAppSelector(state => state.siteAppearance);
    const initApp = useCallback(async () => {
        dispatch(loadingSaveState());
        await dispatch(fetchCurrentUserAsync());
    }, [dispatch]);

    useEffect(() => {
        initApp().then(() => setLoading(false));
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

    if(loading) return <LoadingComponent message='Initialising app...' />

    return (
        <ThemeProvider theme={theme}>
            <ToastContainer position='bottom-center' hideProgressBar theme='colored'/>
            <CssBaseline/>
            <HeaderComponent/>
            <Container>
                <Routes>
                    <Route path='/' element={<MovieHome/>}/>
                    <Route path='/login' element={<Login />}/>
                    <Route path='/register' element={<Register />}/>
                    <Route path='/movie/:name/:id' element={<MovieDetails />}/>
                    <Route path='/category/:name/:id' element={<Category />}/>
                    <Route path='/account' element={
                       <PrivateComponent performRedirect={true}>
                           <FavouriteMovies />
                       </PrivateComponent> 
                    }/>
                    <Route path='/search' element={<MovieSearch />} />
                    <Route path='/server-error' element={<ServerError/>}/>
                    <Route path='*' element={<NotFound/>}/>
                </Routes>
            </Container>
        </ThemeProvider>
    );
}

export default App;
