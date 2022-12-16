import * as React from "react";
import './App.css';
import {Layout, Login} from "./Layout";
import {Admin, Resource} from 'react-admin';
import RestClient from "./Clients/Rest/RestClient";
import {lightTheme} from "./Layout/themes";
import authProvider from "./Providers/AuthProvider";
import {getApiUrl, httpClient} from "./Providers/DataProvider";
import {UsersList} from "./Users/UsersList";
import {UserEdit} from "./Users/UserEdit";
import {UserCreate} from "./Users/UserCreate";

let dataProvider = RestClient(getApiUrl(), httpClient);

const App = () => {
    return (
        <Admin authProvider={authProvider} loginPage={Login} layout={Layout} dataProvider={dataProvider}
               theme={lightTheme} title="Home Budget">
            <Resource name="users" list={UsersList} edit={UserEdit} create={UserCreate}/>
        </Admin>
    );
}

export default App;