import {Grid, Typography} from "@mui/material";
import {MovieDto} from "../../core/models/movieDto";
import {useAppSelector} from "../../core/store/configureStore";
import MovieGridItem from "./MovieGridItem";
import MovieCardSkeleton from "./MovieCardSkeleton";
import {ListView} from "../../core/enums/listView";

interface Props {
    movies: MovieDto[]
}

export default function MovieList({movies} : Props){
    const {moviesLoaded} = useAppSelector(state => state.movie);
    const {listView} = useAppSelector(state => state.siteAppearance);
    
    const viewSize = () => {
        switch(listView){
            case ListView.List:
                return 12;
            default:
                return 4;
        }
    }
    
    const shouldTruncateStoryLine = () => {
        switch(listView){
            case ListView.List:
                return false;
            default:
                return true;
        }
    }
    
    return (
        <Grid container spacing={4} sx={{mb: 1}}>
            {movies.map(movie => (
                <Grid item xs={viewSize()} key={movie.id}>
                    {!moviesLoaded ? (
                        <MovieCardSkeleton />
                    ) : (
                        <MovieGridItem movie={movie} truncateStoryLine={shouldTruncateStoryLine()} />
                    )}
                </Grid>
            ))}
        </Grid>
    )
}