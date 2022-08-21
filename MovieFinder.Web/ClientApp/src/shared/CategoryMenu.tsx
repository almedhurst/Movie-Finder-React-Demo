import {useEffect, useState} from "react";
import {useAppDispatch, useAppSelector} from "../core/store/configureStore";
import {fetchCategoriesAsync} from "../core/slices/categorySlice";
import {Button, Fade, Menu, MenuItem} from "@mui/material";
import {Link, NavLink} from "react-router-dom";
import {urlFriendly} from "../core/utilities/stringUtil";

export default function CategoryMenu() {
    const dispatch = useAppDispatch();
    const {categories, categoriesLoaded} = useAppSelector(state => state.category);

    useEffect(() => {
        if (!categoriesLoaded) dispatch(fetchCategoriesAsync());
    }, [dispatch, categoriesLoaded]);

    const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
    const open = Boolean(anchorEl);
    const handleClick = (event: any) => {
        setAnchorEl(event.currentTarget);
    };
    const handleClose = () => {
        setAnchorEl(null);
    };

    return (
        <>
            <Button
                onClick={handleClick}
                color='inherit'
                sx={{typography: 'h6'}}
            >
                Categories
            </Button>
            <Menu
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                TransitionComponent={Fade}
            >
                {categories.map(item => {
                    return (
                        <MenuItem onClick={handleClose} component={NavLink} to={`/category/${urlFriendly(item.name)}/${item.id}`} key={item.id}>{item.name.toUpperCase()}</MenuItem>
                    )
                })}
            </Menu>
        </>
    )
}