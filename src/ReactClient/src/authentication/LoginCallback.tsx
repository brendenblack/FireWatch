import React from 'react';
import { AuthConsumer } from './AuthContext';

export const LoginCallback = () => (
    <AuthConsumer>
        {({ signinRedirectCallback }) => {
            signinRedirectCallback();
            return <span>loading...</span>
        }}
    </AuthConsumer>
)