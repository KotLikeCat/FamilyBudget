import {getApiUrl} from "./DataProvider";

const authProvider = {
    login: ({username, password}) => {
        const request = new Request(getApiUrl() + '/authentication', {
            method: 'POST',
            body: JSON.stringify({login: username, password}),
            headers: new Headers({'Content-Type': 'application/json'}),
        });
        return fetch(request)
            .then(response => {
                if (response.status === 401) {
                    throw new Error('Wrong login or password');
                }

                if (response.status < 200 || response.status >= 300) {
                    throw new Error("Something went wrong on server side");
                }
                return response.json();
            })
            .then(auth => {
                localStorage.setItem('auth', auth.token);
                localStorage.setItem('username', username);
            })
            .catch((err) => {
                throw new Error(err);
            });
    },
    logout: () => {
        localStorage.removeItem('username');
        localStorage.removeItem('auth');
        return Promise.resolve();
    },
    checkAuth: () =>
        localStorage.getItem('auth') ? Promise.resolve() : Promise.reject(),
    checkError: (error) => {
        const status = error.status;
        if (status === 401) {
            localStorage.removeItem('username');
            localStorage.removeItem('privileges');
            localStorage.removeItem('auth');
            return Promise.reject();
        }
        // other error code (404, 500, etc): no need to log out
        return Promise.resolve();
    },
    getIdentity: () => {
        let user = localStorage.getItem('username');
        return Promise.resolve({
            id: 'user',
            fullName: `${user}`,
        })
    },
    getPermissions: () => Promise.resolve('')
};

export default authProvider;