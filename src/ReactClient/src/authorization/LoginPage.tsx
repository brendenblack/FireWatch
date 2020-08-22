import React from "react";
import { useHistory, useLocation } from "react-router-dom";
import { fakeAuth } from "./fakeAuth";

function LoginPage() {
    let history = useHistory();
    let location = useLocation();
  
    // @ts-ignore
    let { from } = location.state || { from: { pathname: "/" } };
    let login = () => {
        console.log('Redirected from ' + from.pathname);
      fakeAuth.authenticate(() => {
        history.replace(from);
      });
    };
  
    return (
      <div>
        <p>You must log in to view the page at {from.pathname}</p>
        <button onClick={login}>Log in</button>
      </div>
    );
  }

export default LoginPage;