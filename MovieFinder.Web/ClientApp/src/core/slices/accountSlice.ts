import {UserDto} from "../models/userDto";
import {createAsyncThunk, createSlice, isAnyOf} from "@reduxjs/toolkit";
import {FieldValues} from "react-hook-form";
import AccountService from "../services/accountService";
import {FavouriteMovieDto} from "../models/favouriteMovieDto";
import {history} from "../../index";
import {toast} from "react-toastify";
import {AddRemoveFavouriteMovieDto} from "../models/addRemoveFavouriteMovieDto";

interface AccountSlice {
    user: UserDto | null;
    favouriteMovies: FavouriteMovieDto[];
    status: string;
}

const initialState: AccountSlice = {
    user: null,
    favouriteMovies: [],
    status: 'idle'
}
const userLocalStorageKey = "MovieFinderUser";
const authenticationCondition = () => {
    if(!localStorage.getItem(userLocalStorageKey)) return false;
}

export const signInUserAsync = createAsyncThunk<UserDto, FieldValues>(
    'account/signInUserAsync',
    async (data, thunkAPI) => {
        try {
            const userDto = await AccountService.login(data);
            const {favouriteMovies, ...user} = userDto;
            if(favouriteMovies) thunkAPI.dispatch(setFavouriteMovies(favouriteMovies));
            localStorage.setItem(userLocalStorageKey, JSON.stringify(user));
            return user;
            
        } catch(error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }
)

export const fetchCurrentUserAsync = createAsyncThunk<UserDto>(
    'account/fetchCurrentUserAsync',
    async(_, thunkAPI) => {
        thunkAPI.dispatch(setUser(JSON.parse(localStorage.getItem(userLocalStorageKey)!)));
        try {
            const userDto = await AccountService.currentUser();
            const {favouriteMovies, ...user} = userDto;
            if(favouriteMovies) thunkAPI.dispatch(setFavouriteMovies(favouriteMovies));
            localStorage.setItem(userLocalStorageKey, JSON.stringify(user));
            return user;
            
        } catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }, {
        condition: authenticationCondition
    }
)

export const fetchFavouriteMoviesAsync = createAsyncThunk<FavouriteMovieDto[]>(
    'account/fetchFavouriteMoviesAsync',
    async (_, thunkAPI) => {
        thunkAPI.dispatch(setUser(JSON.parse(localStorage.getItem(userLocalStorageKey)!)));
        try {
            return await AccountService.getFavouriteMovie();
        } catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }, {
        condition: authenticationCondition
    }
)

export const addFavouriteMoviesAsync = createAsyncThunk<FavouriteMovieDto[], AddRemoveFavouriteMovieDto>(
    'account/addFavouriteMoviesAsync',
    async (data, thunkAPI) => {
        thunkAPI.dispatch(setUser(JSON.parse(localStorage.getItem(userLocalStorageKey)!)));
        try {
            return await AccountService.addFavouriteMovie(data);
        } catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }, {
        condition: authenticationCondition
    }
)

export const removeFavouriteMoviesAsync = createAsyncThunk<FavouriteMovieDto[], AddRemoveFavouriteMovieDto>(
    'account/removeFavouriteMoviesAsync',
    async (data, thunkAPI) => {
        thunkAPI.dispatch(setUser(JSON.parse(localStorage.getItem(userLocalStorageKey)!)));
        try {
            return await AccountService.deleteFavouriteMovie(data);
        } catch (error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }, {
        condition: authenticationCondition
    }
)

export const accountSlice = createSlice({
    name: 'account',
    initialState,
    reducers:{
        setFavouriteMovies: (state, action) => {
            state.favouriteMovies = action.payload;
        },

        signOut: (state) => {
            state.user = null;
            localStorage.removeItem(userLocalStorageKey);
            history.push('/');
        },
        setUser: (state, action) => {
            state.user = action.payload;
        }
    },
    extraReducers: (builder => {
        builder.addCase(fetchCurrentUserAsync.rejected, (state) => {
            state.user = null;
            localStorage.removeItem(userLocalStorageKey);
            toast.error('Session expired - please login again');
            history.push('/');
        });

        builder.addCase(fetchFavouriteMoviesAsync.fulfilled, (state, action) => {
            state.status = 'idle';
            state.favouriteMovies = action.payload;
        });

        builder.addCase(addFavouriteMoviesAsync.fulfilled, (state, action) => {
            state.status = 'idle';
            state.favouriteMovies = action.payload;
            toast.success("Movie added to favourites")
        });

        builder.addCase(removeFavouriteMoviesAsync.fulfilled, (state, action) => {
            state.status = 'idle';
            state.favouriteMovies = action.payload;
            toast.success("Movie removed from favourites")
        });
        builder.addCase(fetchFavouriteMoviesAsync.pending, (state) => {
            state.status = 'pendingFetchFavouriteMovie';
        })
        
        builder.addMatcher(isAnyOf(signInUserAsync.fulfilled, fetchCurrentUserAsync.fulfilled), (state, action) => {
            state.user = action.payload;
        });
        builder.addMatcher(isAnyOf(signInUserAsync.rejected), (state, action) => {
            throw action.payload;
        });
        builder.addMatcher(isAnyOf(addFavouriteMoviesAsync.pending, removeFavouriteMoviesAsync.pending), (state, action) => {
            state.status = 'pendingFavouriteMovieAction_' + action.meta.arg.titleId;
        });
        builder.addMatcher(isAnyOf(fetchFavouriteMoviesAsync.rejected, addFavouriteMoviesAsync.rejected, removeFavouriteMoviesAsync.rejected), (state) => {
            state.status = 'idle';
            toast.error('Error retrieving favourite movies');
        });
    })
})

export const {setFavouriteMovies, signOut, setUser} = accountSlice.actions;