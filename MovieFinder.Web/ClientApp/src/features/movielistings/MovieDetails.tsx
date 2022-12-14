import {useAppDispatch, useAppSelector} from "../../core/store/configureStore";
import {Link, useParams} from "react-router-dom";
import {fetchMovieAsync, movieSelectors} from "../../core/slices/movieSlice";
import {useEffect} from "react";
import LoadingComponent from "../../shared/LoadingComponent";
import NotFound from "../errors/NotFound";
import {
    Box,
    Chip,
    Divider,
    Grid,
    Paper,
    Table,
    TableBody,
    TableCell,
    TableContainer,
    TableRow, TextField,
    Typography
} from "@mui/material";
import Moment from 'moment';
import {urlFriendly} from "../../core/utilities/stringUtil";
import MovieUserActions from "./MovieUserActions";
import PrivateComponent from "../../shared/PrivateComponent";
import {useForm} from "react-hook-form";
import {LoadingButton} from "@mui/lab";
import {addFavouriteMoviesAsync} from "../../core/slices/accountSlice";
import {FavouriteMovieDto} from "../../core/models/favouriteMovieDto";

export default function MovieDetails() {
    const {id, name} = useParams<{ id: string, name: string }>();

    const dispatch = useAppDispatch();
    const {status: movieStatus} = useAppSelector(state => state.movie);
    const movie = useAppSelector(state => movieSelectors.selectById(state, typeof (id) === 'string' ? id : ''));
    const {favouriteMovies, status} = useAppSelector(state => state.account);
    let favouriteMovie: FavouriteMovieDto | undefined;

    const {register, handleSubmit, setValue, formState: {isSubmitting}} = useForm({
        mode: 'all'
    });
    
    useEffect(() => {
        favouriteMovie = favouriteMovies.find(x => x.movie.id === id);
        setValue('comments', favouriteMovie?.comments);
    }, [favouriteMovies])

    useEffect(() => {
        if (!movie) dispatch(fetchMovieAsync(id ?? ''))
    }, [id, dispatch, movie])
    

    if (movieStatus.includes('pending')) return <LoadingComponent message='Loading movie...'></LoadingComponent>

    if (!movie) return <NotFound></NotFound>

    return (
        <>
            <Typography variant='h1'>{movie.name}</Typography>
            <Grid container spacing={3} component={Paper} sx={{p: 5}}>
                <Grid item xs={10}>
                    {movie.categories.map(item => (
                        <Chip label={item.name.toUpperCase()}
                              size='small' sx={{mb: 1, mr: 1, cursor: 'pointer'}}
                              key={item.id}
                              component={Link} to={`/category/${urlFriendly(item.name)}/${item.id}`}
                        />
                    ))}
                </Grid>
                <Grid item xs={2} sx={{textAlign: 'right'}}>
                    <PrivateComponent performRedirect={false}>
                        <MovieUserActions movieId={movie.id} />
                    </PrivateComponent>
                </Grid>
                <Grid item xs={6}>
                    <Typography>{movie.storyLine}</Typography>
                </Grid>
                <Grid item xs={6}>
                    <TableContainer>
                        <Table size='small'>
                            <TableBody>
                                <TableRow>
                                    <TableCell sx={{fontWeight: 'bold'}}>Year</TableCell>
                                    <TableCell>{movie.year}</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{fontWeight: 'bold'}}>Release Date</TableCell>
                                    <TableCell>{Moment(movie.releaseDate).format('Do MMMM YYYY')}</TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{fontWeight: 'bold'}}>Duration</TableCell>
                                    <TableCell>{movie.runTime} minutes</TableCell>
                                </TableRow>

                                <TableRow>
                                    <TableCell sx={{fontWeight: 'bold'}}>Directors</TableCell>
                                    <TableCell>
                                        {movie.directors.map(item => (
                                            <Typography key={item.id}>{item.name}</Typography>
                                        ))}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{fontWeight: 'bold'}}>Writers</TableCell>
                                    <TableCell>
                                        {movie.writers.map(item => (
                                            <Typography key={item.id}>{item.name}</Typography>
                                        ))}
                                    </TableCell>
                                </TableRow>
                                <TableRow>
                                    <TableCell sx={{fontWeight: 'bold'}}>Actors</TableCell>
                                    <TableCell>
                                        {movie.actors.map(item => (
                                            <Typography key={item.id}>{item.name}</Typography>
                                        ))}
                                    </TableCell>
                                </TableRow>
                            </TableBody>
                        </Table>
                    </TableContainer>
                </Grid>
                <PrivateComponent performRedirect={false}>
                    <Grid item xs={12} sx={{mt: 1}}>
                        <Divider sx={{mb:1}} />
                        <Box component='form' noValidate
                             onSubmit={handleSubmit(async (data) => {
                                 await dispatch(addFavouriteMoviesAsync({
                                     titleId: movie.id,
                                     comments: data.comments
                                 }));
                             })}
                        >
                            <TextField
                                label='Your Comments'
                                multiline
                                fullWidth
                                rows={6}
                                {...register('comments', {
                                    value: favouriteMovie?.comments
                                })} />
                            <Box sx={{textAlign: 'right' }}>
                                <LoadingButton
                                    loading={isSubmitting || status.startsWith(`pendingFavouriteMovieAction_${movie.id}`)}
                                    type='submit'
                                    variant='contained'
                                    sx={{mt: 1}}
                                >
                                    Save
                                </LoadingButton>
                            </Box>
                        </Box>
                    </Grid>
                </PrivateComponent>
            </Grid>
        </>
    )
}