import {createAsyncThunk, createEntityAdapter, createSlice} from "@reduxjs/toolkit";
import { RootState } from "../store/configureStore";
import {CategoryMovieDto} from "../models/categoryMovieDto";
import MovieService from "../services/movieService";

interface RandomMovieState {
    randomMoviesLoaded: boolean;
    status: string;
}

const randomMovieAdapter = createEntityAdapter<CategoryMovieDto>();

export const fetchRandomMoviesAsync = createAsyncThunk(
    'randomMovies/getRandomMovies',
    async(_, thunkAPI) => {
        try {
                return MovieService.GetRandomMoviesByRandomCategories();
        } catch(error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }
)

export const randomMovieSlice = createSlice({
    name: 'movie',
    initialState: randomMovieAdapter.getInitialState<RandomMovieState>({
        randomMoviesLoaded: false,
        status: 'idle'
    }),
    reducers:{},
    extraReducers:(builder => {
        builder.addCase(fetchRandomMoviesAsync.pending, (state) => {
            state.status = 'pendingFetchRemoveMovies';
        });

        builder.addCase(fetchRandomMoviesAsync.fulfilled, (state, action) => {
            randomMovieAdapter.setAll(state, action.payload);
            state.randomMoviesLoaded = true;
            state.status = 'idle';
        });

        builder.addCase(fetchRandomMoviesAsync.rejected, (state, action) => {
            console.log(action.payload);
            state.status = 'idle';
        });
    })
});

export const randomMovieSelectors = randomMovieAdapter.getSelectors((state: RootState) => state.randomMovie);
export const {} = randomMovieSlice.actions;