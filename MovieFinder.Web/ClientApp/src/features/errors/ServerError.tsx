import {Button, Container, Divider, Paper, Typography} from "@mui/material";
import {useLocation, useNavigate} from "react-router-dom";
import {history} from "../../index";

export default function ServerError() {
    
    const state : any = useLocation().state;
    
    return (
        <Container component={Paper}>
            {state ? (
                <>
                    <Typography variant='h3' color='error' gutterBottom>{state.title}</Typography>
                    <Divider></Divider>
                    <Typography>{state.detail || 'Internal server error'}</Typography>
                </>
            ) : (
                <Typography variant='h5' gutterBottom>Server error</Typography>
            )}
            <Button onClick={() => history.push('/')}>Go back to the home page</Button>
        </Container>
    );
}