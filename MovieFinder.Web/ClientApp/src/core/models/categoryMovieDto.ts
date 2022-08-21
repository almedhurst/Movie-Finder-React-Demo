import {MovieDto} from "./movieDto";

export interface CategoryMovieDto {
    id: string;
    name: string;
    movies: MovieDto[];
}