import ApiUtil from "../utilities/apiUtil";

const AccountService = {
    login: (values: any) => ApiUtil.post('Account/Login', values),
    currentUser: () => ApiUtil.get('Account/GetCurrentUser'),
    GetFavouriteMovie: () => ApiUtil.get('Account/GetFavouriteMovies'), 
    AddFavouriteMovie: (values: any) => ApiUtil.post('Account/AddFavouriteMovie', values),
    DeleteFavouriteMovie: (values: any) => ApiUtil.post('Account/DeleteFavouriteMovie', values),
}

export default AccountService;