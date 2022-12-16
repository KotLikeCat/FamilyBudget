import * as React from "react";
import {
    List,
    Datagrid,
    TextField,
    TextInput
} from 'react-admin';

const postFilters = [
    <TextInput label="User" name="user" source="user" alwaysOn/>,
];
export const BudgetsList = () => {
    return (
        <>
            <List sort={{field: 'createdAt', order: 'DESC'}} filters={postFilters} empty={false}>
                <Datagrid rowClick="edit">
                    <TextField source="name" label="Name"/>
                    <TextField source="description" label="Description"/>
                    <TextField source="createTime" label="Created At"/>
                    <TextField source="owner" label="Owner"/>
                </Datagrid>
            </List>
        </>
    );
}