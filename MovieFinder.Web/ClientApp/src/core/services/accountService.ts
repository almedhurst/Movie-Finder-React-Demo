import ApiUtil from "../utilities/apiUtil";

const AccountService = {
    login: (values: any) => ApiUtil.post('Account/Login', values),
    currentUser: () => ApiUtil.get('Account/GetCurrentUser'),
    getFavouriteMovie: () => ApiUtil.get('Account/GetFavouriteMovies'), 
    addFavouriteMovie: (values: any) => ApiUtil.post('Account/AddFavouriteMovie', values),
    deleteFavouriteMovie: (values: any) => ApiUtil.post('Account/DeleteFavouriteMovie', values),
    register: (values: any) => ApiUtil.post('Account/Register', values)
}

export default AccountService;