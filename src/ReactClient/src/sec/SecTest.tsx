import React, { useState, useEffect, Component } from 'react';
import { useAuthState, AuthContext } from '../authentication/AuthContext';
import { User } from 'oidc-client';
import { WeatherForecastClient, TodoListsClient, AccountsClient } from '../firewatch-service.g';

interface SecTestState {
    user: User | null;
}

class SecTest extends Component<{}, SecTestState> {
    constructor(props: any) {
        super(props);

        this.state = {
            user: null
        }
    }   

    componentDidMount() {
        const client = new AccountsClient();
        client.getAccounts().then(vm => console.log(vm));
    }

    render() {
        return (
            <div className="max-w-screen p-8">
                <h1>Sec page</h1>
                <p>Access token:</p> {this.state.user &&  <pre className="truncate">{this.state.user.access_token}</pre>}
            </div>
        );
    }
}

SecTest.contextType = AuthContext;

export default SecTest;

const SecTest2 = function () {
    const [ user, setUser ] = useState<User | null>();
    const auth = useAuthState();
    useEffect(() => {
        // auth.getUser().then(user => setUser(user));
    });
    
    // const { user } = useAuthState();
    // console.log('user');

    return (
        <div>
            <h1>Sec</h1>
            {user && <h2>{user.access_token}</h2>}
        </div>);
}


