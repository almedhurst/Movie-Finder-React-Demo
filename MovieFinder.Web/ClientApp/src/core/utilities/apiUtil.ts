import axios, {Axios, AxiosError, AxiosResponse} from "axios";
import { toast } from "react-toastify";
import {useNavigate} from "react-router-dom";
import {history} from "../../index";
import {PageinatedDto} from "../models/PaginatedDto";


const sleep = () => new Promise(resolve => setTimeout(resolve, 1000));

axios.defaults.baseURL = '/api/';

const responseBody = (response: AxiosResponse) => response.data;

axios.interceptors.response.use(async response => {

    await sleep();
    const pagination = response.headers['pagination'];
    if(pagination){
        response.data = new PageinatedDto(response.data, JSON.parse(pagination));
        return response;
    }
    return response;
}, (error : AxiosError<any>) => {
    const {data, status} = error.response!;
    switch(status){
        case 400:
            if(data.errors){
                const modelStateErrors: string[] = [];
                for(const key in data.errors){
                    if(data.errors[key]){
                        modelStateErrors.push(data.errors[key])
                    }
                }
                throw modelStateErrors.flat();
            }
            toast.error(data.title);
            break;
        case 401:
            toast.error(data.title);
            break;
        case 500:
            history.push('/server-error', data);
            break;
        default:
            break;
    }
    return Promise.reject(error.response);
})

const ApiUtil = {
    get: (url: string, params?: URLSearchParams) => axios.get(url, {params}).then(responseBody),
    post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
    put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
    delete: (url: string) => axios.delete(url).then(responseBody),
}

export default ApiUtil;