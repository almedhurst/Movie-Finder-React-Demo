import {MovieOrderBy} from "../enums/movieOrderBy";

export interface MovieParams {
    orderBy: MovieOrderBy;
    categories?: string[];
    minYear?: number;
    maxYear?: number;
    pageNumber: number;
    pageSize: number;
}