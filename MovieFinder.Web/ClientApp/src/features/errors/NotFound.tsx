import {Button, Container, Divider, Paper, Typography} from "@mui/material";
import {Link} from "react-router-dom";

export default function NotFound(){
    return (
        <Container component={Paper}>
            <Typography gutterBottom variant='h3'>Oops - we could not find what you are looking for</Typography>
            <Divider></Divider>
            <Button fullWidth component={Link} to='/'>Go back to the home page</Button>
        </Container>
    )
}