import * as React from "react";
import {
    Create,
    SimpleForm,
    TextInput,
    required,regex
} from 'react-admin';
import {SelectField, SelectArrayInput, ReferenceInput, SelectInput} from 'react-admin';
import {Box} from "@mui/material";
import {useParams} from "react-router-dom";

const AmountValidation = [required(), regex(/^[0-9]+(\.[0-9]{1,2})?$/, 'Should be valid amount')]

export const BudgetDetailCreate = () => {
    const {budgetId} = useParams();
    return (
    <Create resource={`budgetDetails/${budgetId}`}>
        <SimpleForm>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="description" source="description"/>
                <SelectInput fullWidth name="isIncome" validate={required()} source="type" choices={[
                    { id: true, name: 'Income' },
                    { id: false, name: 'Expense' },
                ]} />
                <ReferenceInput source="categoryId" reference="categories">
                    <SelectInput fullWidth label="Category" optionText="name" optionValue="id" validate={required()}/>
                </ReferenceInput>
                <TextInput name="amount" source="amount" validate={AmountValidation}/>
            </Box>
        </SimpleForm>
    </Create>)
};