// A wrapper for <Route> that redirects to the login

import React from "react";
import { Route, Redirect } from "react-router-dom";
import { fakeAuth } from "./fakeAuth";


// screen if you're not yet authenticated.
//@ts-ignore
function PrivateRoute({ children, ...rest }) {
    return (
      <Route
        {...rest}
        render={({ location }) =>
          fakeAuth.isAuthenticated ? (
            children
          ) : (
            <Redirect
              to={{
                pathname: "/login",
                state: { from: location }
              }}
            />
          )
        }
      />
    );
  }

export default PrivateRoute;