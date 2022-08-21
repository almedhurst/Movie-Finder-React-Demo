import {FieldValues, useForm} from "react-hook-form";
import {useLocation, useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../core/store/configureStore";
import {Avatar, Box, Container, Grid, Paper, TextField, Typography} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import LockOutlinedIcon from '@mui/icons-material/LockOutlined';
import {signInUserAsync} from "../../core/slices/accountSlice";

export default function Login(){
    const navigate = useNavigate();
    const location = useLocation() as any;
    const dispatch = useAppDispatch();
    const {register, handleSubmit, formState: {isSubmitting, errors, isValid}} = useForm({
        mode: 'all'
    });

    async function submitForm(data: FieldValues){
        try {
            await dispatch(signInUserAsync(data));
            navigate(location.state?.from?.pathname || '/account');
        } catch(error){
            console.log(error);
        }
    }

    return (
        <Container component={Paper} maxWidth="sm" sx={{display: 'flex', flexDirection: 'column', alignItems:'center', p: 4}}>

            <Avatar sx={{ m: 1, bgcolor: 'secondary.main' }}>
                <LockOutlinedIcon />
            </Avatar>
            <Typography component="h1" variant="h5">
                Sign in
            </Typography>
            <Box component="form" onSubmit={handleSubmit(submitForm)} noValidate sx={{ mt: 1 }}>
                <TextField
                    margin="normal"
                    fullWidth
                    label="Username"
                    autoFocus
                    {...register('username', {
                        required: 'Username is required'
                    })}
                    error={!!errors.username}
                    helperText={errors?.username?.message?.toString()}
                />
                <TextField
                    margin="normal"
                    fullWidth
                    label="Password"
                    type="password"
                    {...register('password', {
                        required: 'Password is required'
                    })}
                    error={!!errors.password}
                    helperText={errors?.password?.message?.toString()}
                />
                <LoadingButton
                    disabled={!isValid}
                    loading={isSubmitting}
                    type="submit"
                    fullWidth
                    variant="contained"
                    sx={{ mt: 3, mb: 2 }}
                >
                    Sign In
                </LoadingButton>
            </Box>
        </Container>
    );
}