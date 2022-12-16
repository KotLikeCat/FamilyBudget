import * as React from "react";
import {
    Create,
    SimpleForm,
    TextInput,
    required
} from 'react-admin';
import {ReferenceArrayInput, SelectArrayInput} from 'react-admin';
import {Box} from "@mui/material";

export const BudgetCreate = () => (
    <Create>
        <SimpleForm>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="name" source="name" validate={required()}/>
                <TextInput name="description" source="description"/>
                <ReferenceArrayInput source="userIds" reference="users" filter={{ hideCaller: true }}>
                    <SelectArrayInput label="Access Users" optionText="login" optionValue="id"/>
                </ReferenceArrayInput>
            </Box>
        </SimpleForm>
    </Create>
);