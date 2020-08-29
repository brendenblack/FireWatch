import React, { useContext } from 'react';
import { HomePage } from './pub/home/HomePage';
import { Link, Switch, Route } from 'react-router-dom';
import cx from "classnames"
import { LoginPage } from './pub/LoginPage';
import { LoginCallback } from './authentication/LoginCallback';
import { LogoutCallback } from './authentication/LogoutCallback';
import { IDENTITY_CONFIG } from './authentication/authConstants';
import { AuthContext } from './authentication/AuthContext';
import { Navbar } from './common/Navbar';
import SecTest from './sec/SecTest';
import { PrivateRoute } from './authentication/PrivateRoute';
import { SilentRenew } from './authentication/SilentRenew';

function UnauthenticatedApp() {
    const [ isOpen, setOpen] = React.useState<boolean>(false);
    const context = useContext(AuthContext);

    // fetch(`${IDENTITY_CONFIG.authority}/.well-known/openid-configuration`)
    //     .then(resp => resp.json()).then(json => console.log(json));

    return (
        <div className="min-h-screen">
            <Navbar />
            
            <div className="h-screen" style={{ paddingTop: '54px', marginTop: '-54px' }}>
                <Switch>
                    <Route exact path="/signin-oidc" component={LoginCallback} />
                    <Route exact path="/signout-callback-oidc" component={LogoutCallback} />
                    <Route exact path="/silent" component={SilentRenew} />
                    {/* <Route exact={true} path="/register" component={Register} /> */}
                    {/* <Route exact={true} path="/logout" component={Logout} />
                    <Route exact={true} path="/logout/callback" component={LogoutCallback} />    
                    <Route exact={true} path="/silentrenew" component={SilentRenew} /> */}
                    <Route path="/" exact component={HomePage} />
                    <Route path="/login" component={LoginPage} />
                    <PrivateRoute path="/en/dashboard" component={SecTest} />
                    {/* <Route path="/demo" component={DemoPage} /> */}

                </Switch>
            </div>
        </div>
    );
}

export default UnauthenticatedApp;