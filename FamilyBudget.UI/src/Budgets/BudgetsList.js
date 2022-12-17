import * as React from "react";
import {
    List,
    Datagrid,
    TextField
} from 'react-admin';
import { FilterLiveSearch} from 'react-admin';
import { Card, CardContent } from '@mui/material';
import {DetailsButton} from "./Buttons/DetailsButton";

const PostFilterSidebar = () => (
    <Card sx={{ order: -1, mr: 2, mt: 8, width: 200 }}>
        <CardContent>
            <FilterLiveSearch source="name" label="Name" variant="outlined"/>
            <FilterLiveSearch source="description" label="Description" variant="outlined"/>
            <FilterLiveSearch source="owner" label="Owner" variant="outlined"/>
        </CardContent>
    </Card>
);

export const BudgetsList = () => {
    return (
        <>
            <List sort={{field: 'createdAt', order: 'DESC'}} empty={false} aside={<PostFilterSidebar />}>
                <Datagrid rowClick="edit">
                    <TextField source="name" label="Name"/>
                    <TextField source="description" label="Description"/>
                    <TextField source="createdAt" label="Created At"/>
                    <TextField source="owner" label="Owner"/>
                    <DetailsButton label="Details"/>
                </Datagrid>
            </List>
        </>
    );
}