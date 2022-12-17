import * as React from "react";
import {
    List,
    Datagrid,
    TextField,
    FilterList, FilterListItem, FunctionField
} from 'react-admin';
import {Link, useParams} from "react-router-dom";
import {TopToolbar, FilterLiveSearch, CreateButton, Button} from 'react-admin';
import {Card, CardContent} from '@mui/material';
import {useBasename} from "ra-core/src/routing/useBasename";

const ListActions = () => {
    const basename = useBasename();
    return (<TopToolbar>
        <CreateButton/>
        {/* Add your custom actions */}
        <Button
            component={Link}
            to={`${basename}/categories`}
            label="Categories"
        >
        </Button>
    </TopToolbar>);
};

const PostFilterSidebar = () => (
    <Card sx={{order: -1, mr: 2, mt: 8, width: 200}}>
        <CardContent>
            <FilterLiveSearch source="description" label="Description" variant="outlined"/>
            <FilterLiveSearch source="category" label="Category" variant="outlined"/>
            <FilterLiveSearch source="owner" label="Owner" variant="outlined"/>
            <FilterList label="Type">
                <FilterListItem label="Income" value={{ type: 'income' }} />
                <FilterListItem label="Expense" value={{ type: 'expense' }} />
            </FilterList>
        </CardContent>
    </Card>
);

export const BudgetDetailsList = () => {
    const {budgetId} = useParams();
    return (
        <>
            <List title='Details' sort={{field: 'createdAt', order: 'DESC'}} actions={<ListActions/>} resource={`budgetDetails/${budgetId}`}
                  empty={false} aside={<PostFilterSidebar/>}>
                <Datagrid rowClick="edit">
                    <TextField source="description" label="Description"/>
                    <TextField source="amount" label="Amount"/>
                    <FunctionField
                        source="isIncome"
                        label="Type"
                        render={record => `${record.isIncome ? 'Income' : 'Expense'}`}
                    />;
                    <TextField source="category" label="Category"/>
                    <TextField source="owner" label="Owner"/>
                </Datagrid>
            </List>
        </>
    );
}