import * as React from "react";
import {
    Edit,
    SimpleForm,
    TextInput,
    required, useRecordContext
} from 'react-admin';
import {ReferenceArrayInput, SelectArrayInput} from 'react-admin';
import {Box} from "@mui/material";

const Transform = (data) => {
    if (data.owner || data.owner == null) {
        delete data.owner;
    }

    if (data.createTime) {
        delete data.createTime;
    }

    if (data.id) {
        delete data.id;
    }

    return data
};

const BudgetTitle = () => {
    const record = useRecordContext();
    return <span>{record ? `${record.name}` : ''}</span>;
};

export const BudgetEdit = () => (
    <Edit title={<BudgetTitle/>} transform={Transform}>
        <SimpleForm>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="name" source="name" validate={required()}/>
                <TextInput name="description" source="description"/>
                <ReferenceArrayInput source="userIds" reference="users" filter={{ hideCaller: true }}>
                    <SelectArrayInput label="Access Users" optionText="login" optionValue="id"/>
                </ReferenceArrayInput>
            </Box>
        </SimpleForm>
    </Edit>
);