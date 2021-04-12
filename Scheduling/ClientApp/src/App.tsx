import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import User from './components/User';

import './custom.css'


export default () => (
    <Layout>
        <Route exact path='/' component={User} />
    </Layout>
);
