import * as React from "react";
import {
    Edit,
    SimpleForm,
    TextInput,
    PasswordInput,
    required, useRecordContext, minLength
} from 'react-admin';
import {Box} from "@mui/material";

const PostTitle = () => {
    const record = useRecordContext();
    return <span>User {record ? `"${record.login}"` : ''}</span>;
};

const Transform = (data) => {
    if (data.lastLoginAt) {
        delete data.lastLoginAt;
    }

    if (data.createdAt) {
        delete data.createdAt;
    }

    if (data.id) {
        delete data.id;
    }

    if (data.login) {
        delete data.login;
    }

    return data
};

const validatePassword = [required(), minLength(6)];

export const UserEdit = () => (
    <Edit title={<PostTitle/>} transform={Transform}>
        <SimpleForm>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="login" disabled source="login"/>
                <PasswordInput name="password" source="password" validate={validatePassword}/>
            </Box>
        </SimpleForm>
    </Edit>
);