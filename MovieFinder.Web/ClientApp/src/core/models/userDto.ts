import {FavouriteMovieDto} from "./favouriteMovieDto";

export interface UserDto {
    email: string;
    token: string;
    givenName: string;
    favouriteMovies: FavouriteMovieDto[];
}