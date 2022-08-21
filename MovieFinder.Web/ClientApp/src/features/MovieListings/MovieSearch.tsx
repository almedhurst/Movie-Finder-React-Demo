import {useSearchParams} from "react-router-dom";
import {Box, Grid, Paper, ToggleButtonGroup, Typography} from "@mui/material";
import {useAppDispatch, useAppSelector} from "../../core/store/configureStore";
import {useEffect} from "react";
import {fetchCategoriesAsync} from "../../core/slices/categorySlice";
import CheckboxSelectList from "../../shared/CheckboxSelectList";
import {fetchMoviesAsync, movieSelectors, setMovieParams, setPageNumber} from "../../core/slices/movieSlice";
import MinMaxSliders from "../../shared/MinMaxSliders";
import LoadingComponent from "../../shared/LoadingComponent";
import MovieList from "./MovieList";
import ListModeSelector from "../../shared/ListModeSelector";
import PaginationComponent from "../../shared/PaginationComponent";
import {Pages} from "@mui/icons-material";
import SelectList from "../../shared/SelectList";
import {MovieOrderBy} from "../../core/enums/movieOrderBy";

export default function MovieSearch() {
    const movies = useAppSelector(movieSelectors.selectAll);
    const dispatch = useAppDispatch();
    const {categories, categoriesLoaded} = useAppSelector(state => state.category);
    const {movieParams, moviesLoaded, paginationMeta} = useAppSelector(state => state.movie);

    useEffect(() => {
        if (!moviesLoaded) dispatch(fetchMoviesAsync());
    }, [dispatch, moviesLoaded])

    useEffect(() => {
        if (!categoriesLoaded) dispatch(fetchCategoriesAsync());
    }, [dispatch, categoriesLoaded]);

    if (!categoriesLoaded) return <LoadingComponent message='Loading movies...'></LoadingComponent>

    return (
        <>
            <Typography variant='h1'>Search Movies</Typography>
            <Grid container columnSpacing={4}>
                <Grid item xs={3}>
                    <Paper sx={{mb: 2, p: 2}}>
                        <SelectList items={[
                                        {value: MovieOrderBy.DateDesc.toString(), text: 'Date Descending'},
                                        {value: MovieOrderBy.DateAsc.toString(), text: 'Date Ascending'},
                                        {value: MovieOrderBy.Name.toString(), text: 'Name'},
                                        {value: MovieOrderBy.Runtime.toString(), text: 'Runtime'},
                                    ]}
                                    label='Sorting'
                                    onChange={(item: string) => dispatch(setMovieParams({orderBy: item}))}
                                    selectedItem={movieParams.orderBy.toString()}
                                    fullWidth={true}
                        />
                    </Paper>

                    <Paper sx={{mb: 2, p: 2}}>
                        <CheckboxSelectList items={categories.map(category => {
                            return {value: category.id, text: category.name}
                        })}
                                            onChange={(items: string[]) => dispatch(setMovieParams({categories: items}))}
                                            label='Categories' selectedItems={movieParams.categories}/>
                    </Paper>

                    <Paper sx={{mb: 2, p: 2}}>
                        <MinMaxSliders minValue={1960} maxValue={2022}
                                       onChange={(min: number, max: number) => dispatch(setMovieParams({
                                           minYear: min,
                                           maxYear: max
                                       }))}
                                       selectedValue={{min: movieParams.minYear, max: movieParams.maxYear}}
                                       label='Release Year'/>
                    </Paper>
                </Grid>
                <Grid item xs={9}>
                    <ListModeSelector pageSizeValue={movieParams.pageSize}
                                      onPageSizeChange={(pageSize: string) => dispatch(setMovieParams({pageSize: pageSize}))}/>
                    <MovieList movies={movies}/>
                    {paginationMeta &&
                        <PaginationComponent
                            paginationData={paginationMeta}
                            onPageChange={(page: number) => dispatch(setPageNumber({pageNumber: page}))}
                        />
                    }
                </Grid>
            </Grid>
        </>
    )
}