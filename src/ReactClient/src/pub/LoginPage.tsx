import React from 'react';
import { Link } from 'react-router-dom';
import { AuthConsumer } from '../authentication/AuthContext';

interface LoginPageState {
    username: string;
    password: string;
    isRememberMe: boolean;
}

// export function LoginPage() {
//     const auth = useAuthState();

//     auth.authService.signinRedirect();

//     return <div>Redirecting to sign in...</div>
// }

export function LoginPage() {
    console.log('Rendering login page');

    return (
        <AuthConsumer>
            {({ signinRedirect }) => {
                console.log('Calling signinRedirect', signinRedirect);
                signinRedirect();
                return <span>loading...</span>
            }}
        </AuthConsumer>);
};

// export default class OldLoginPage extends React.Component<{}, LoginPageState> {
//     constructor(props: {}) {
//         super(props);
//         this.state = {
//             username: "",
//             password: "",
//             isRememberMe: false,
//         };

//         this.setUsername = this.setUsername.bind(this);
//         this.setPassword = this.setPassword.bind(this);
//         this.handleClick = this.handleClick.bind(this);

//     }


//     render() {
//         // const authService = new AuthService();
//         // authService.signinRedirect();

//         return <div>Loading from old login...</div>;
//     }

//     setUsername(username: string) {
//         this.setState({
//             ...this.state,
//             username: username,
//         });
//     }

//     setPassword(password: string) {
//         this.setState({
//             ...this.state,
//             password: password,
//         });
//     }

//     handleClick() {
//         // const { authService } = useAuthState();

//         // const authService = new AuthService();
//         // authService.signinRedirect();
//         // console.log('submitting form');


//         // fetch('', {
//         //     method: 'POST',
//         //     headers: {
//         //         'Content-Type': 'application/json',
//         //     },
//         //     credentials: 'include',
//         //     body: JSON.stringify({
//         //         username: this.state.username,
//         //         password: this.state.password,
//         //         returnUrl: '',
//         //     })
//         // });
//     }

//     old_render() {
//         return (
//             <div className="w-full flex items-center justify-center h-full bg-gray-300">
//                 <form className="bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 w-1/3 sm:w-full md:w-auto lg:w-1/3 xl:w-1/4">
//                     <div className="mb-4">
//                         <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="username">Username</label>
//                         <input type="text" 
//                                name="username" 
//                                id="username" 
//                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 leading-tight focus:outline-none focus:shadow-outline" 
//                                value={this.state.username} 
//                                onChange={e => this.setUsername(e.target.value)} 
//                         />
//                     </div>
//                     <div className="mb-6">
//                         <label className="block text-gray-700 text-sm font-bold mb-2" htmlFor="password">Password</label>
//                         <input type="password" 
//                                name="password" 
//                                id="password" 
//                                className="shadow appearance-none border rounded w-full py-2 px-3 text-gray-700 mb-3 leading-tight focus:outline-none focus:shadow-outline"
//                                value={this.state.password} 
//                                onChange={e => this.setPassword(e.target.value)}  />
//                     </div>
//                     <div className="flex items-center justify-between">
//                         <button type="button" 
//                                 name="button" 
//                                 id="login" 
//                                 className="bg-blue-500 hover:bg-blue-700 text-white font-bold py-2 px-4 rounded focus:outline-none focus:shadow-outline"
//                                 onClick={this.handleClick}>Login
//                         </button>
//                         <Link to="/forgot" className="inline-block align-baseline font-bold text-sm text-blue-500 hover:text-blue-800">
//                            Forgot Password?
//                         </Link>
//                     </div>
                    
//                 </form>
//             </div>
//         );
//     }
// }