import {fetchUtils} from "react-admin";

const httpClient = (url, options = {}) => {
    if (!options.headers) {
        options.headers = new Headers({Accept: 'application/json'});
    }
    const token = localStorage.getItem('auth');
    options.headers.set('Authorization', `Bearer ${token}`);
    return fetchUtils.fetchJson(url, options);
};

const getApiUrl = () => {
    return 'http://localhost:7080/api';
}

export {httpClient, getApiUrl}