import React, { Component } from 'react'
import AuthService from './authService';
import { User } from 'oidc-client';

// /*
//  * https://kentcdodds.com/blog/authentication-in-react-applications
//  * https://codesandbox.io/s/react-app-auth-bc99t?fontsize=14&hidenavigation=1&theme=dark&file=/src/better.js:100-166
//  * https://medium.com/@franciscopa91/how-to-implement-oidc-authentication-with-react-context-api-and-react-router-205e13f2d49
//  */

export const AuthContext = React.createContext({
    signinRedirectCallback: function() { },
    logout: () => ({}),
    signoutRedirectCallback: () => ({}),
    isAuthenticated: () => ({}),
    signinRedirect: function() { console.log('default') },
    signinSilentCallback: function() { console.log('sign in silent default') },
    createSigninRequest: () => ({}),
    getUser: (): Promise<User | null> => new Promise(res => { }) 
});

export const AuthConsumer = AuthContext.Consumer;

export class AuthProvider extends Component {
    authService: AuthService;
    constructor(props: any) {
        super(props);
        this.authService = new AuthService();
    }
   
    render() {
        const value = {
            signinRedirectCallback: () => this.authService.signinRedirectCallback(),
            logout: () => this.authService.logout,
            signoutRedirectCallback: () => this.authService.signoutRedirectCallback,
            isAuthenticated: () => this.authService.isAuthenticated(),
            signinRedirect: () => this.authService.signinRedirect(),
            signinSilentCallback: this.authService.signinSilentCallback,
            createSigninRequest: () => this.authService.createSigninRequest,
            getUser: () => this.authService.getUser()
        };
        return <AuthContext.Provider value={value}>{this.props.children}</AuthContext.Provider>;
    }
}

export function useAuthState() {
    const state = React.useContext(AuthContext);
    // const isPending = state.status === 'pending'
    // const isError = state.status === 'error'
    // const isSuccess = state.status === 'success'
    const isAuthenticated = state.isAuthenticated();

    return {
      ...state,
      isAuthenticated,
    }
}



// type AuthContextProps = { 
//     status: string,
//     user: Oidc.User | null,
//     authService: AuthService,
// };

// const AuthContext = React.createContext<AuthContextProps>({ status: 'pending', user: null, authService: new AuthService() });

// // const AuthContext = React.createContext({
// //     signinRedirectCallback: () => ({}),
// //     logout: () => ({}),
// //     signoutRedirectCallback: () => ({}),
// //     isAuthenticated: () => ({}),
// //     signinRedirect: () => ({}),
// //     signinSilentCallback: () => ({}),
// //     createSigninRequest: () => ({})
// // });

// export const AuthConsumer = AuthContext.Consumer;


// interface AuthProviderState {
//     status: string;
//     error: string | null;
//     user: Oidc.User | null;
//     authService: AuthService;
// }

// export function AuthProvider(props: PropsWithChildren<{}>) {

//     const [ state, setState ] = React.useState<AuthProviderState>({
//         status: 'pending',
//         error: null,
//         user: null,
//         authService: new AuthService(),
//     });

//     React.useEffect(() => {
//         console.log('fetching user');
//         state.authService.getUser().then(
//             user => setState({ ...state, status: 'success', error: null, user }),
//             error => setState({ ...state, status: 'error', error: error.message, user: null }),
//         )
//     });

//     return (
//         <AuthContext.Provider value={state}>
//             {state.status === 'pending' 
//                 ? ('Fetching auth...')
//                 : state.status === 'error' ? (
//                     <div>
//                         Oh no.
//                         <div>
//                             <pre>{state.error}</pre>
//                         </div>
//                     </div>
//                 ) : (
//                     props.children
//                 )}
//         </AuthContext.Provider>
//     );
// }

// export function useAuthState() {
//     const state = React.useContext(AuthContext)
//     const isPending = state.status === 'pending'
//     const isError = state.status === 'error'
//     const isSuccess = state.status === 'success'
//     const isAuthenticated = state.user && isSuccess
//     return {
//       ...state,
//       isPending,
//       isError,
//       isSuccess,
//       isAuthenticated,
//     }
// }




