import {
    FormControl,
    Grid,
    InputLabel,
    MenuItem,
    Select,
    SelectChangeEvent,
    ToggleButton,
    ToggleButtonGroup
} from "@mui/material";
import {useAppDispatch, useAppSelector} from "../core/store/configureStore";
import {ListView} from "../core/enums/listView";
import ViewListIcon from '@mui/icons-material/ViewList';
import ViewModuleIcon from '@mui/icons-material/ViewModule';
import {setListView} from "../core/slices/siteAppearanceSlice";
import {setMovieParams} from "../core/slices/movieSlice";
import {useState} from "react";

interface Props {
    pageSizeValue: number;
    onPageSizeChange: (selection: string) => void;
}

export default function ListModeSelector({pageSizeValue, onPageSizeChange}: Props){

    const [pageSize, setPageSize] = useState(pageSizeValue || 9);
    
    const dispatch = useAppDispatch();
    const {listView} = useAppSelector(state => state.siteAppearance);

    const handleListViewChange = (event: React.MouseEvent<HTMLElement>, nextView: ListView) => {
        dispatch(setListView(nextView));
    };
    
    const handlePageSizeChange = (event: SelectChangeEvent) => {
        setPageSize(parseInt(event.target.value));
        onPageSizeChange(event.target.value);
    }
    
    return (
        <Grid container sx={{mb: 1}}>
            <Grid item xs={6}>
                <FormControl size='small' sx={{minWidth: 100}}>
                    <InputLabel id='page-size-label'>Page Size</InputLabel>
                    <Select value={pageSize.toString()}
                            onChange={handlePageSizeChange}
                            labelId='page-size-label'
                            label='Page Size'
                    >
                        <MenuItem value={6}>6</MenuItem>
                        <MenuItem value={9}>9</MenuItem>
                        <MenuItem value={12}>12</MenuItem>
                        <MenuItem value={18}>18</MenuItem>
                        <MenuItem value={24}>24</MenuItem>
                    </Select>
                </FormControl>
            </Grid>
            <Grid item xs={6} sx={{textAlign: 'right'}}>
                <ToggleButtonGroup
                    color='primary'
                    value={listView}
                    exclusive
                    onChange={handleListViewChange}
                    size='small'
                >
                    <ToggleButton value={ListView.List}>
                        <ViewListIcon />
                    </ToggleButton>
                    <ToggleButton value={ListView.Grid}>
                        <ViewModuleIcon />
                    </ToggleButton>
                </ToggleButtonGroup>
            </Grid>
        </Grid>
    )
}