import { AppBar, Box, FormControlLabel, FormGroup, styled, Switch, Toolbar, Typography } from "@mui/material";
import { NavLink } from "react-router-dom";
import { setDarkMode } from "../core/slices/siteAppearanceSlice";
import {useAppDispatch, useAppSelector} from "../core/store/configureStore";

const navStyles = {
    textDecoration: 'none',
    color: 'inherit',
    typography: 'h6',
    '&:hover': {
        color: 'grey.500'
    }
}

const Offset = styled('div')(({ theme }) => theme.mixins.toolbar);

export default function HeaderComponent(){
    const dispatch = useAppDispatch();
    const {darkMode} = useAppSelector(state => state.siteAppearance);
    
    const ChangeDarkMode = (event: any) => {
        dispatch(setDarkMode(event.target.checked));
    }
    
    return(
        <>
            <AppBar position='fixed'>
                <Toolbar sx={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                    <Box display='flex' alignItems='center'>
                        <Typography variant='h6' component={NavLink} to='/' sx={navStyles}>
                            Movie finder
                        </Typography>
                    </Box>
                    <Box display='flex' alignItems='center'>
                        <FormGroup>
                            <FormControlLabel control={<Switch
                                checked={darkMode}
                                onChange={ChangeDarkMode}></Switch>}
                                              label='Dark Mode'
                                              labelPlacement='start'/>
                        </FormGroup>
                    </Box>
                </Toolbar>
            </AppBar>
            <Offset />
        </>
    )
}