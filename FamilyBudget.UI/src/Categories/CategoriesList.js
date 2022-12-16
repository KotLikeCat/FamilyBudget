import * as React from "react";
import {
    List,
    Datagrid,
    TextField
} from 'react-admin';

export const CategoriesList = () => {
    return (
        <>
            <List sort={{field: 'name', order: 'DESC'}} empty={false}>
                <Datagrid rowClick="edit" bulkActionButtons={false}>
                    <TextField source="name" label="Name"/>
                </Datagrid>
            </List>
        </>
    );
}