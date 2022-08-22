import {CircularProgress, IconButton} from "@mui/material";
import FavoriteIcon from '@mui/icons-material/Favorite';
import FavoriteBorderIcon from '@mui/icons-material/FavoriteBorder';
import {useAppDispatch, useAppSelector} from "../../core/store/configureStore";
import {addFavouriteMoviesAsync, removeFavouriteMoviesAsync} from "../../core/slices/accountSlice";

interface Props {
    movieId: string;
}

export default function MovieUserActions ({movieId}: Props){
    const dispatch = useAppDispatch();
    const {favouriteMovies, status} = useAppSelector(state => state.account);
    
    const favouriteMovieItem = favouriteMovies.find(x => x.movie.id == movieId);
    
    const view = () => (favouriteMovieItem ? (
        <IconButton onClick={() => dispatch(removeFavouriteMoviesAsync({titleId: movieId}))}>
            <FavoriteIcon />
        </IconButton>
    ) : (
        <IconButton onClick={() => dispatch(addFavouriteMoviesAsync({titleId: movieId}))}>
            <FavoriteBorderIcon />
        </IconButton>
    ))

    
    return (
        <>
            {status.startsWith(`pendingFavouriteMovieAction_${movieId}`) ? (
                <CircularProgress size='2rem' />
            ) : (
                view()
            )}
            
        </>
    )
}