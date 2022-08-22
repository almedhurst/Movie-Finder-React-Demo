import {Grid, Typography} from "@mui/material";
import {useAppSelector} from "../../core/store/configureStore";
import MovieGridItem from "../movielistings/MovieGridItem";

export default function FavouriteMovies(){
    const {favouriteMovies} = useAppSelector(state => state.account);
    return (
        <>
            <Typography variant='h1'>Favourite Movies</Typography>
            {!favouriteMovies || favouriteMovies.length == 0 ? (
              <Typography>You have no favourite movies</Typography>  
            ) : (
                <Grid container spacing={4}>
                    {favouriteMovies.map(item => (
                        <Grid item xs={12} key={item.movie.id}>
                            <MovieGridItem movie={item.movie} truncateStoryLine={false} />
                        </Grid>
                    ))}
                </Grid>    
            )}
        </>
    )
}