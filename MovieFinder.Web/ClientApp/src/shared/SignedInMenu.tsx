import {useAppDispatch, useAppSelector} from "../core/store/configureStore";
import {useState} from "react";
import {Button, Fade, Menu, MenuItem} from "@mui/material";
import {signOut} from "../core/slices/accountSlice";
import {Link} from "react-router-dom";

export default function SignedInMenu(){
    const dispatch = useAppDispatch();
    const {user} = useAppSelector(state => state.account);
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
                sx={{typography: 'h6', textTransform: 'none'}}
            >
                {user?.givenName}
            </Button>
            <Menu
                anchorEl={anchorEl}
                open={open}
                onClose={handleClose}
                TransitionComponent={Fade}
            >
                <MenuItem component={Link} to='/account' onClick={handleClose}>Favourite Movies</MenuItem>
                <MenuItem onClick={() => {
                    dispatch(signOut());
                }}>Logout</MenuItem>
            </Menu>
        </>
    )
}