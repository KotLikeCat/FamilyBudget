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
import {BudgetsList} from "./Budgets/BudgetsList";
import {BudgetCreate} from "./Budgets/BudgetCreate";
import {BudgetEdit} from "./Budgets/BudgetEdit";
import {CategoriesList} from "./Categories/CategoriesList";
import {CategoryCreate} from "./Categories/CategoryCreate";
import {CategoryEdit} from "./Categories/CategoryEdit";
import {BudgetDetailsList} from "./BudgetsDetails/BudgetDetailsList";
import {BudgetDetailCreate} from "./BudgetsDetails/BudgetDetailCreate";
import {BudgetDetailEdit} from "./BudgetsDetails/BudgetDetailEdit";

let dataProvider = RestClient(getApiUrl(), httpClient);

const App = () => {
    return (
        <Admin authProvider={authProvider} loginPage={Login} layout={Layout} dataProvider={dataProvider}
               theme={lightTheme} title="Home Budget">
            <Resource name="users" list={UsersList} edit={UserEdit} create={UserCreate}/>
            <Resource name="budgets" list={BudgetsList} create={BudgetCreate} edit={BudgetEdit}/>
            <Resource name="categories" list={CategoriesList} create={CategoryCreate} edit={CategoryEdit}/>
            <Resource name="budgetDetails/:budgetId" list={BudgetDetailsList} create={BudgetDetailCreate} edit={BudgetDetailEdit}/>
        </Admin>
    );
}

export default App;