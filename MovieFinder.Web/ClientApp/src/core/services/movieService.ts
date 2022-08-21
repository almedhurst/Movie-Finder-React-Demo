import ApiUtil from "../utilities/apiUtil";

const MovieService = {
    GetRandomMoviesByRandomCategories: () => ApiUtil.get('Movies/GetRandomMoviesByRandomCategories'),
    GetMovie: (movieId: string) => ApiUtil.get(`Movies/GetMovie?movieId=${movieId}`),
    SearchMovies: (params: URLSearchParams) => ApiUtil.get('/Movies/GetMovies', params)
}

export default MovieService;