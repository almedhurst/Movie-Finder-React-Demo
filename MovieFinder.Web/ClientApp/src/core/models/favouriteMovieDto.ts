import {MovieDto} from "./movieDto";

export interface FavouriteMovieDto{
    comments: string | null;
    movie: MovieDto;
}