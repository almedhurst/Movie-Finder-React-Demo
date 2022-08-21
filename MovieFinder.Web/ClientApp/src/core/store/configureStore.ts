import {configureStore} from "@reduxjs/toolkit";
import {TypedUseSelectorHook, useDispatch, useSelector} from "react-redux";
import {siteAppearanceSlice} from "../slices/siteAppearanceSlice";
import {categorySlice} from "../slices/categorySlice";
import {randomMovieSlice} from "../slices/randomMovieSlice";
import {movieSlice} from "../slices/movieSlice";

export const store = configureStore({
    reducer: {
        siteAppearance: siteAppearanceSlice.reducer,
        category: categorySlice.reducer,
        randomMovie: randomMovieSlice.reducer,
        movie: movieSlice.reducer
    }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;

export const useAppDispatch = () => useDispatch<AppDispatch>();
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector;