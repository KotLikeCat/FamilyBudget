import * as React from 'react';
import Box from '@mui/material/Box';
import {useAuthenticated} from 'react-admin'
import DataUsageIcon from '@mui/icons-material/DataUsage';
import {
    useTranslate,
    MenuItemLink,
    MenuProps,
    useSidebarState,
} from 'react-admin';


const Menu = ({dense = false}: MenuProps) => {
    useTranslate();
    const [open] = useSidebarState();
    useAuthenticated();

    return (
        <>
            <Box
                sx={{
                    width: open ? 250 : 50,
                    marginTop: 1,
                    marginBottom: 1,
                    transition: theme =>
                        theme.transitions.create('width', {
                            easing: theme.transitions.easing.sharp,
                            duration: theme.transitions.duration.leavingScreen,
                        }),
                }}
            >
                <MenuItemLink
                    to="/users"
                    state={{_scrollToTop: true}}
                    primaryText='Users'
                    dense={dense}
                    leftIcon={<DataUsageIcon/>}
                />
            </Box>
        </>
    );
};

export default Menu;