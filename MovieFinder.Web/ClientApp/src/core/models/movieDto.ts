import {CategoryDto} from "./categoryDto";
import {NameDto} from "./nameDto";

export interface MovieDto {
    id: string;
    name: string;
    year: number;
    runTime: number;
    releaseDate: Date;
    storyLine: string;
    categories: CategoryDto[];
    directors: NameDto[];
    writers: NameDto[];
    actors: NameDto[];
}