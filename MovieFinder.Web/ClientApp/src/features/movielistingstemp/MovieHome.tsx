import {useAppDispatch, useAppSelector} from "../../core/store/configureStore";
import {fetchRandomMoviesAsync, randomMovieSelectors} from "../../core/slices/randomMovieSlice";
import {useEffect} from "react";
import LoadingComponent from "../../shared/LoadingComponent";
import {Card, CardContent, Grid, Typography} from "@mui/material";
import MovieGridItem from "./MovieGridItem";

export default function MovieHome(){
    const randomMovies = useAppSelector(randomMovieSelectors.selectAll);
    const {randomMoviesLoaded} = useAppSelector(state => state.randomMovie);
    const dispatch = useAppDispatch();
    
    useEffect(() => {
        if(!randomMoviesLoaded) dispatch(fetchRandomMoviesAsync());
    },[randomMoviesLoaded, dispatch])
    
    if(!randomMoviesLoaded) return <LoadingComponent message='Loading movies...'></LoadingComponent>
    
    return (
        <>
            <Typography variant='h1'>Movie Finder</Typography>
            {randomMovies.map(item => (
                <Card sx={{mb: 2}} key={item.id}>
                    <CardContent>
                        <Grid container>
                            <Grid item xs={9}>
                                <Typography variant='h3'>{item.name.toUpperCase() }</Typography>
                            </Grid>
                            <Grid item xs={3} sx={{textAlign: 'right'}}>
                                <Typography>Show All</Typography>
                            </Grid>
                        </Grid>
                        <Grid container spacing={3}>
                            {item.movies.map(movie => (
                                <Grid item xs={3} key={movie.id}>
                                    <MovieGridItem movie={movie} truncateStoryLine={true} />
                                </Grid>
                            ))}
                        </Grid>
                    </CardContent>
                </Card>
            ))}
            
        </>
        
        
        
    )
}