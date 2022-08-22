import {AppBar, Box, Button, FormControlLabel, FormGroup, styled, Switch, Toolbar, Typography} from "@mui/material";
import {NavLink} from "react-router-dom";
import {setDarkMode} from "../core/slices/siteAppearanceSlice";
import {useAppDispatch, useAppSelector} from "../core/store/configureStore";
import {fetchCategoriesAsync} from "../core/slices/categorySlice";
import {useEffect} from "react";
import CategoryMenu from "./CategoryMenu";
import SignedInMenu from "./SignedInMenu";
import PrivateComponent from "./PrivateComponent";

const navStyles = {
    textDecoration: 'none',
    color: 'inherit',
    typography: 'h6',
    '&:hover': {
        color: 'grey.500'
    }
}

const Offset = styled('div')(({theme}) => theme.mixins.toolbar);

export default function HeaderComponent() {
    const dispatch = useAppDispatch();
    const {darkMode} = useAppSelector(state => state.siteAppearance);
    const {user} = useAppSelector(state => state.account);


    const ChangeDarkMode = (event: any) => {
        dispatch(setDarkMode(event.target.checked));
    }


    return (
        <>
            <AppBar position='fixed'>
                <Toolbar sx={{display: 'flex', justifyContent: 'space-between', alignItems: 'center'}}>
                    <Box display='flex' alignItems='center'>
                        <Typography variant='h6' component={NavLink} to='/' sx={navStyles}>
                            MOVIE FINDER
                        </Typography>
                    </Box>
                    <Box display='flex' alignItems='center'>
                        <CategoryMenu/>
                        <Button
                            color='inherit'
                            sx={{typography: 'h6'}}
                            component={NavLink}
                            to='/search'
                        >
                            Search
                        </Button>
                    </Box>
                    <Box display='flex' alignItems='center'>
                        {!user && (
                            <>
                                <Button
                                    color='inherit'
                                    sx={{typography: 'h6'}}
                                    component={NavLink}
                                    to='/login'
                                >
                                    Sign in
                                </Button>
                                <Button
                                    color='inherit'
                                    sx={{typography: 'h6'}}
                                    component={NavLink}
                                    to='/register'
                                >
                                    Register
                                </Button>
                            </>
                        )}
                        <PrivateComponent performRedirect={false}>
                            <SignedInMenu/>
                        </PrivateComponent>
                        <FormGroup>
                            <FormControlLabel control={<Switch
                                checked={darkMode}
                                onChange={ChangeDarkMode}></Switch>}
                                              label='DARK MODE'
                                              labelPlacement='start'/>
                        </FormGroup>
                    </Box>
                </Toolbar>
            </AppBar>
            <Offset/>
        </>
    )
}