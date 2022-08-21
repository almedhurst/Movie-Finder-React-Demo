import {Grid, Typography} from "@mui/material";
import {useParams} from "react-router-dom";
import {useAppDispatch, useAppSelector} from "../../core/store/configureStore";
import {useEffect} from "react";
import {fetchCategoriesAsync} from "../../core/slices/categorySlice";
import NotFound from "../errors/NotFound";
import LoadingComponent from "../../shared/LoadingComponent";
import {
    fetchMoviesAsync,
    fetchMoviesByCategoryAsync,
    movieSelectors,
    setMovieParams,
    setPageNumber
} from "../../core/slices/movieSlice";
import ListModeSelector from "../../shared/ListModeSelector";
import MovieList from "./MovieList";
import PaginationComponent from "../../shared/PaginationComponent";
import {MovieOrderBy} from "../../core/enums/movieOrderBy";
import SelectList from "../../shared/SelectList";

export default function Category() {
    const {id, name} = useParams<{ id: string, name: string }>();
    const dispatch = useAppDispatch();

    const movies = useAppSelector(movieSelectors.selectAll);
    const {categories, categoriesLoaded} = useAppSelector(state => state.category);
    const {movieParams, moviesLoaded, paginationMeta} = useAppSelector(state => state.movie);
    
    const categoryItem = categories.find(c => c.id == id);

    useEffect(() => {
        const categoryItem = categories.find(c => c.id == id);
        dispatch(setMovieParams({categories: [id]}));
    }, [dispatch, id, categories])
    
    useEffect(() => {
        if (!moviesLoaded) dispatch(fetchMoviesByCategoryAsync(id!));
    }, [dispatch, moviesLoaded])

    useEffect(() => {
        if (!categoriesLoaded) dispatch(fetchCategoriesAsync());
    }, [dispatch, categoriesLoaded]);
    
    if(!categoriesLoaded) return <LoadingComponent message='Loading movies...'></LoadingComponent>;
    
    if(!categoryItem) return <NotFound/>;
    
    return (
        <Grid container columnSpacing={4}>
            <Grid item xs={12}>
                <Typography variant='h1'>{categoryItem.name}</Typography>
            </Grid>

            <Grid item xs={12} sx={{textAlign: 'right'}}>
                <SelectList items={[
                    {value: MovieOrderBy.DateDesc.toString(), text: 'Date Descending'},
                    {value: MovieOrderBy.DateAsc.toString(), text: 'Date Ascending'},
                    {value: MovieOrderBy.Name.toString(), text: 'Name'},
                    {value: MovieOrderBy.Runtime.toString(), text: 'Runtime'},
                ]}
                            label='Sorting'
                            onChange={(item: string) => dispatch(setMovieParams({orderBy: item}))}
                            selectedItem={movieParams.orderBy.toString()}
                            fullWidth={false}/>
            </Grid>
            <Grid item xs={12}>
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
    )
}