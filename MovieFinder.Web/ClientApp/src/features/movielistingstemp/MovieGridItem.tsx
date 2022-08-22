import {MovieDto} from "../../core/models/movieDto";
import {Box, Button, Card, CardActions, CardContent, Chip, Grid, Stack, Typography} from "@mui/material";
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import {truncate, urlFriendly} from "../../core/utilities/stringUtil";
import {Link} from "react-router-dom";
import PrivateComponent from "../../shared/PrivateComponent";
import MovieUserActions from "./MovieUserActions";

interface Props {
    movie: MovieDto;
    truncateStoryLine: boolean;
}

export default function MovieGridItem({movie, truncateStoryLine}: Props) {
    return (
        <Card variant='outlined' sx={{height: '100%', display: 'flex', flexDirection: 'column'}}>
            <CardContent>
                <Typography variant="h5" component="div">
                    {movie.name}
                </Typography>
                <Typography variant="body2">
                    {truncateStoryLine ? (
                        truncate(movie.storyLine, 100)    
                    ) : (
                        movie.storyLine
                    )}
                    
                </Typography>
            </CardContent>
            <Box sx={{marginTop: 'auto'}}>
                <CardContent sx={{paddingBottom: 0}}>
                    <Grid container sx={{alignItems: 'center'}} color="text.secondary">
                        <Grid item xs={12}>
                            {movie.categories.map(item => (
                                <Chip label={item.name.toUpperCase()} 
                                      size='small' sx={{mb: 1, mr: 1, cursor: 'pointer'}} 
                                      key={item.id}
                                      component={Link} to={`/category/${urlFriendly(item.name)}/${item.id}`}
                                />
                            ))}
                        </Grid>
                        <Grid item xs={6}>{movie.year}</Grid>
                        <Grid item xs={6} sx={{textAlign: 'right', display: 'flex', justifyContent: 'right'}}>
                            <AccessTimeIcon/>
                            <Typography>{movie.runTime}m</Typography>
                        </Grid>
                    </Grid>
                </CardContent>
                <CardActions>
                    <Grid container>
                        <Grid item xs={9}>
                            <Button size="small" component={Link} to={`/movie/${urlFriendly(movie.name)}/${movie.id}`}>Learn
                                More</Button>
                        </Grid>
                        <Grid item xs={3} sx={{textAlign: 'right'}}>
                            <PrivateComponent performRedirect={false}>
                                <MovieUserActions movieId={movie.id} />
                            </PrivateComponent>
                        </Grid>
                    </Grid>
                    
                </CardActions>
            </Box>
        </Card>
    )
}