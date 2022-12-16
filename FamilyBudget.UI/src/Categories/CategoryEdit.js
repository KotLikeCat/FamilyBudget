import * as React from "react";
import {
    Edit,
    SimpleForm,
    TextInput,
    required, maxLength, Toolbar, SaveButton
} from 'react-admin';
import {Box} from "@mui/material";

const validateCategoryName = [required(), maxLength(50)];
const EditToolBar = props => (
    <Toolbar {...props} >
        <SaveButton/>
    </Toolbar>
);

export const CategoryEdit = () => (
    <Edit>
        <SimpleForm toolbar={<EditToolBar/>}>
            <Box sx={{width: '200px', margin: '1em'}}>
                <TextInput name="name" source="name" validate={validateCategoryName}/>
            </Box>
        </SimpleForm>
    </Edit>
);