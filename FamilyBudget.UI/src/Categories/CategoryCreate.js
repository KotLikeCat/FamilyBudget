import * as React from "react";
import {
    Create,
    SimpleForm,
    TextInput,
    required, maxLength
} from 'react-admin';
import {Box} from "@mui/material";

const validateCategoryName = [required(), maxLength(50)];

export const CategoryCreate = () => (
    <Create>
        <SimpleForm>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="name" source="name" validate={validateCategoryName}/>
            </Box>
        </SimpleForm>
    </Create>
);