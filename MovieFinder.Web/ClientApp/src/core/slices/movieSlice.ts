import {MovieDto} from "../models/movieDto";
import {createAsyncThunk, createEntityAdapter, createSlice, isAnyOf} from "@reduxjs/toolkit";
import movieService from "../services/movieService";
import {RootState} from "../store/configureStore";
import {MovieParams} from "../models/movieParams";
import {Movie} from "@mui/icons-material";
import {MovieOrderBy} from "../enums/movieOrderBy";
import {PaginationMetaDto} from "../models/paginationMetaDto";

interface MovieState {
    status: string;
    movieParams: MovieParams;
    moviesLoaded: boolean;
    paginationMeta: PaginationMetaDto | null;
}

const movieAdapter = createEntityAdapter<MovieDto>();

const getAxiosParams = (movieParams: MovieParams) =>{
    const params = new URLSearchParams();
    params.append('OrderBy', movieParams.orderBy.toString());
    params.append('pageNumber', movieParams.pageNumber.toString());
    params.append('pageSize', movieParams.pageSize.toString());

    if(movieParams.categories && movieParams.categories.length > 0) params.append('Categories', movieParams.categories.toString());
    if(movieParams.minYear) params.append('MinYear', movieParams.minYear.toString());
    if(movieParams.maxYear) params.append('MaxYear', movieParams.maxYear.toString());
    
    return params;
}

export const fetchMovieAsync = createAsyncThunk<MovieDto, string>(
    'movie/fetchMovieAsync',
    async (movieId, thunkAPI) => {
        try {
            return await movieService.GetMovie(movieId);
        } catch(error:any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }
)

export const fetchMoviesAsync = createAsyncThunk<MovieDto[], void, {state:RootState}>(
    'movie/fetchMoviesAsync',
    async(_, thunkAPI) => {
        const params = getAxiosParams(thunkAPI.getState().movie.movieParams);
        try {
            const response = await movieService.SearchMovies(params);
            thunkAPI.dispatch(setPaginationMeta(response.paginationMeta));
            return response.data;
        } catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

export const fetchMoviesByCategoryAsync = createAsyncThunk<MovieDto[], string, {state:RootState}>(
    'movie/fetchMoviesByCategoryAsync',
    async(categoryId, thunkAPI) => {
        thunkAPI.dispatch(setMovieParams({categories: [categoryId]}));
        const params = getAxiosParams(thunkAPI.getState().movie.movieParams);
        try {
            const response = await movieService.SearchMovies(params);
            thunkAPI.dispatch(setPaginationMeta(response.paginationMeta));
            return response.data;
        } catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data})
        }
    }
)

function initParams() {
    return {
        orderBy: MovieOrderBy.DateDesc,
        pageNumber: 1,
        pageSize: 9
    }
}

export const movieSlice = createSlice({
    name: 'movie',
    initialState: movieAdapter.getInitialState<MovieState>({
        status: 'idle',
        movieParams: initParams(),
        moviesLoaded: false,
        paginationMeta: null
    }),
    reducers:{
        setMovieParams: (state, action) => {
            state.moviesLoaded = false;
            state.movieParams = {...state.movieParams, ...action.payload, pageNumber: 1}
        },
        setPageNumber: (state, action) => {
            state.moviesLoaded = false;
            state.movieParams = {...state.movieParams, ...action.payload}
        },
        resetMovieParams: (state) => {
            state.movieParams = initParams();
        },
        setPaginationMeta: (state, action) => {
            state.paginationMeta = action.payload
        }
    },
    extraReducers:(builder => {
        
        builder.addCase(fetchMovieAsync.pending, (state) => {
            state.status = 'pendingFetchMovie';
        });
        builder.addCase(fetchMovieAsync.fulfilled, (state, action) => {
            movieAdapter.upsertOne(state, action.payload);
            state.status = 'idle';
        });
        builder.addCase(fetchMovieAsync.rejected, (state, action) => {
            console.log(action.payload);
            state.status = 'idle';
        });


        builder.addMatcher(isAnyOf(fetchMoviesAsync.pending, fetchMoviesByCategoryAsync.pending), (state) => {
            state.status = 'pendingFetchMovies';
        });
        builder.addMatcher(isAnyOf(fetchMoviesAsync.fulfilled, fetchMoviesByCategoryAsync.fulfilled), (state, action) => {
            movieAdapter.setAll(state, action.payload);
            state.status = 'idle';
            state.moviesLoaded = true;
        });
        builder.addMatcher(isAnyOf(fetchMoviesAsync.rejected, fetchMoviesByCategoryAsync.rejected ), (state, action) => {
            console.log(action.payload)
            state.status = 'idle';
        });
    })
})

export const movieSelectors = movieAdapter.getSelectors((state: RootState) => state.movie)
export const {setMovieParams, setPageNumber, resetMovieParams, setPaginationMeta} = movieSlice.actions;