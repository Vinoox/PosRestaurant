import axios, {type AxiosResponse } from 'axios';
import { type User, type SelectRestaurantDto, type ContextToken } from '../models/user';


axios.defaults.baseURL = 'http://localhost:5278/api';

axios.interceptors.request.use(config => {
    const token = localStorage.getItem('jwt');
    if (token && config.headers) config.headers.Authorization = `Bearer ${token}`;
    return config;
});

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    del: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Auth = {
    login: (values: any) => requests.post<User>('/auth/authenticate', values),
    register: (values: any) => requests.post<void>('/auth/register', values), // Nowe
    selectRestaurant: (data: SelectRestaurantDto) => requests.post<ContextToken>('/auth/select-restaurant', data),
};

const Restaurants = {
    create: (values: { name: string }) => requests.post<any>('/restaurants', values) // Nowe
};

const agent = {
    Auth,
    Restaurants
};

export default agent;