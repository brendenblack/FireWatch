import React, { PropsWithChildren } from 'react'
import { UserManager } from 'oidc-client';

/*
 * https://kentcdodds.com/blog/authentication-in-react-applications
 * https://codesandbox.io/s/react-app-auth-bc99t?fontsize=14&hidenavigation=1&theme=dark&file=/src/better.js:100-166
 */

type AuthContextProps = { 
    status: string,
    user: Oidc.User | null
};

const AuthContext = React.createContext<AuthContextProps>({ status: 'pending', user: null });

type User = { id: string; }

interface AuthProviderProps {

}

interface AuthProviderState {
    status: string;
    error: string | null;
    user: Oidc.User | null;
}

const config = {
    authority: "https://localhost:5001",
    client_id: "js",
    redirect_uri: "https://localhost:5003/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "https://localhost:5003/index.html",
}

const mgr = new UserManager(config);


const sleep = (time: number) => new Promise(resolve => setTimeout(resolve, time));

// const getUser = () => sleep(1000).then(() => ({ id: 'elmo'}));

const getUser = () => {
    return mgr.getUser().then(user => {
        if (user) {
            return user;
        } else {
            return null;
        }
    })
}

function AuthProvider(props: PropsWithChildren<AuthProviderProps>) {

    const [ state, setState ] = React.useState<AuthProviderState>({
        status: 'pending',
        error: null,
        user: null,
    });

    React.useEffect(() => {
        console.log('fetching user');
        mgr.getUser().then(
            user => setState({ status: 'success', error: null, user }),
            error => setState({ status: 'error', error: error.message, user: null }),
        )
    }, []);

    return (
        <AuthContext.Provider value={state}>
            {state.status === 'pending' 
                ? ('Fetching auth...')
                : state.status === 'error' ? (
                    <div>
                        Oh no.
                        <div>
                            <pre>{state.error}</pre>
                        </div>
                    </div>
                ) : (
                    props.children
                )}
        </AuthContext.Provider>
    );




}

const useAuth = () => React.useContext(AuthContext);

function useAuthState() {
    const state = React.useContext(AuthContext)
    const isPending = state.status === 'pending'
    const isError = state.status === 'error'
    const isSuccess = state.status === 'success'
    const isAuthenticated = state.user && isSuccess
    return {
      ...state,
      isPending,
      isError,
      isSuccess,
      isAuthenticated,
    }
  }

export { AuthProvider, useAuth, useAuthState };