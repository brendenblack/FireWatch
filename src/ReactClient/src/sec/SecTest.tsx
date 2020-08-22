import React from 'react';
import { useAuthState } from '../authorization/AuthContext';

const SecTest = function () {
    const { user } = useAuthState();
    console.log('user');

    return <h1>Sec</h1>;
}

export default SecTest;