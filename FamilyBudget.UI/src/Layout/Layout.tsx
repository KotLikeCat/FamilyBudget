import * as React from 'react';
import {Layout, LayoutProps} from 'react-admin';
import AppBar from './AppBar';
import Menu from './Menu';

// eslint-disable-next-line import/no-anonymous-default-export
export default (props: LayoutProps) => {
    return <Layout {...props} appBar={AppBar} menu={Menu}/>;
};