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
export const UsersList = () => {
    return (
        <>
            <List sort={{field: 'createdAt', order: 'DESC'}} filters={postFilters} empty={false}>
                <Datagrid rowClick="edit">
                    <TextField source="login" label="Login"/>
                    <TextField source="createdAt" label="Created"/>
                    <TextField source="lastLoginAt" label="Last Login"/>
                </Datagrid>
            </List>
        </>
    );
}