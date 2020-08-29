import React from "react";
import { AuthConsumer } from "../authentication/AuthContext";


export const Logout = () => (
    <AuthConsumer>
        {({ logout }) => {
            logout();
            return <span>loading</span>;
        }}
    </AuthConsumer>
);
