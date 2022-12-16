import * as React from "react";
import {
    Create,
    SimpleForm,
    TextInput,
    PasswordInput,
    required, minLength
} from 'react-admin';
import {Box} from "@mui/material";

const validatePassword = [required(), minLength(6)];

export const UserCreate = () => (
    <Create>
        <SimpleForm>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="login" source="login" validate={required()}/>
                <PasswordInput name="password" source="password" validate={validatePassword}/>
            </Box>
        </SimpleForm>
    </Create>
);