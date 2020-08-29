import React from "react";
import { Route } from "react-router-dom";
import { AuthConsumer } from "./AuthContext";

//@ts-ignore
export const PrivateRoute = ({ component, ...rest }) => {
  //@ts-ignore
  const renderFn = (Component) => (props) => (
      <AuthConsumer>
          {({ isAuthenticated, signinRedirect }) => {
              if (!!Component && isAuthenticated()) {
                  return <Component {...props} />;
              } else {
                  signinRedirect();
                  return <span>Redirecting to log in...</span>;
              }
          }}
      </AuthConsumer>
  );

  return <Route {...rest} render={renderFn(component)} />;
};


// screen if you're not yet authenticated.
//@ts-ignore
// function PrivateRoute({ children, ...rest }) {
//     return (
//       <Route
//         {...rest}
//         render={({ location }) =>
//           fakeAuth.isAuthenticated ? (
//             children
//           ) : (
//             <Redirect
//               to={{
//                 pathname: "/login",
//                 state: { from: location }
//               }}
//             />
//           )
//         }
//       />
//     );
//   }

// export default PrivateRoute;