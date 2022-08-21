import {createAsyncThunk, createEntityAdapter, createSlice} from "@reduxjs/toolkit";
import {NameDto} from "../models/nameDto";
import CategoryService from "../services/categoryService";
import { RootState } from "../store/configureStore";

interface CategoryState {
    categoriesLoaded: boolean;
    status: string;
    categories: NameDto[];
}

const categoryAdapter = createEntityAdapter<NameDto>();

export const fetchCategoriesAsync = createAsyncThunk(
    'categories/getCategories',
    async(_, thunkAPI) => {
        try {
                return CategoryService.GetCategories();
        } catch(error: any){
            return thunkAPI.rejectWithValue({error: error.data});
        }
    }
)

export const categorySlice = createSlice({
    name: 'category',
    initialState: categoryAdapter.getInitialState<CategoryState>({
        categoriesLoaded: false,
        status: 'idle',
        categories: []
    }),
    reducers:{},
    extraReducers:(builder => {
        builder.addCase(fetchCategoriesAsync.pending, (state) => {
            state.status = 'pendingFetchCategories';
        });

        builder.addCase(fetchCategoriesAsync.fulfilled, (state, action) => {
            state.categories = action.payload
            state.categoriesLoaded = true;
            state.status = 'idle';
        });

        builder.addCase(fetchCategoriesAsync.rejected, (state, action) => {
            console.log(action.payload);
            state.status = 'idle';
        });
    })
});

export const categorySelectors = categoryAdapter.getSelectors((state: RootState) => state.category);
export const {} = categorySlice.actions;