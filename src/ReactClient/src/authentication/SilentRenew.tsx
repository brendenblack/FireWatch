import React from "react";
import { AuthConsumer } from "./AuthContext";

export const SilentRenew = () => (
    <AuthConsumer>
        {({ signinSilentCallback }) => {
            console.log('Silently renewing');
            signinSilentCallback();
            return <span>loading</span>;
        }}
    </AuthConsumer>
);
