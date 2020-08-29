import React from 'react';
import { AuthProvider } from './authentication/AuthContext';
import UnauthenticatedApp from './UnauthenticatedApp';
import { Navbar } from './common/Navbar';
import { Switch, Route } from 'react-router-dom';
import { PrivateRoute } from './authentication/PrivateRoute';
import { LoginCallback } from './authentication/LoginCallback';
import { LogoutCallback } from './authentication/LogoutCallback';
import { SilentRenew } from './authentication/SilentRenew';
import { Logout } from './pub/Logout';
import { LoginPage } from './pub/LoginPage';
import { HomePage } from './pub/home/HomePage';
import SecTest from './sec/SecTest';


function App() {
	// const authService = new AuthService();
	// const AuthenticatedApp = React.lazy(() => import('./AuthenticatedApp'));
	// const UnauthenticatedApp = React.lazy(() => import('./UnauthenticatedApp'));

	return (
		<AuthProvider>
			 <div className="min-h-screen">
            <Navbar />
            
            <div className="h-screen" style={{ paddingTop: '54px', marginTop: '-54px' }}>
                <Switch>
                    <Route exact path="/signin-oidc" component={LoginCallback} />
                    <Route exact path="/silent" component={SilentRenew} />
					<Route exact path="/logout" component={Logout} />
					<Route exact path="/logout/callback" component={LogoutCallback} />   
                    <Route path="/" exact component={HomePage} />
                    <Route path="/login" component={LoginPage} />
                    <PrivateRoute path="/en/dashboard" component={SecTest} />
                    {/* <Route path="/demo" component={DemoPage} /> */}

                </Switch>
            </div>
        </div>
		</AuthProvider>);

	// const { user } = useAuthState();
	// console.log('User', user);

	// return user ? <AuthenticatedApp /> : <UnauthenticatedApp />;

	// return (
	// 	<div>
	// 		<header className="bg-gray-900">
	// 			<div className="flex items-center justify-between px-4 py-3">
	// 				<div>
	// 					<h1 className="font-sans text-xl text-gray-300">Firewatch</h1>
	// 				</div>
	// 				<div>
	// 					<button type="button" className="text-gray-500 block focus:text-white hover:text-white" onClick={() => setOpen(!isOpen)}>
	// 						<svg className={`${cx({ "hidden": !isOpen })} h-6 w-6 fill-current`} viewBox="0 0 24 24">
	// 							<path fillRule="evenodd" d="M18.278 16.864a1 1 0 0 1-1.414 1.414l-4.829-4.828-4.828 4.828a1 1 0 0 1-1.414-1.414l4.828-4.829-4.828-4.828a1 1 0 0 1 1.414-1.414l4.829 4.828 4.828-4.828a1 1 0 1 1 1.414 1.414l-4.828 4.829 4.828 4.828z"/>
	// 						</svg>
	// 						<svg className={`${cx({ "hidden": isOpen })} h-6 w-6 fill-current`} viewBox="0 0 24 24">
	// 							<path fillRule="evenodd" d="M4 5h16a1 1 0 0 1 0 2H4a1 1 0 1 1 0-2zm0 6h16a1 1 0 0 1 0 2H4a1 1 0 0 1 0-2zm0 6h16a1 1 0 0 1 0 2H4a1 1 0 0 1 0-2z"/>
	// 						</svg>
	// 					</button>
	// 				</div>
	// 			</div>
	// 			<div className={`${cx({ "block": isOpen, "hidden": !isOpen })} px-2 pt-2 pb-4`}>
	// 				{routes.map((route, index) => {
	// 					const linkClassNames = cx({
	// 						"mt-1": index === 0
	// 					});
	// 					return (
	// 						<NavLink 
	// 							key={index} 
	// 							exact={route.isExact}  
	// 							className={`${linkClassNames} block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1`}
	// 							to={route.path} >
	// 							{route.title}
	// 						</NavLink>
	// 					);
	// 				})}
	// 			</div>
	// 		</header>
	// 	<main>
	// 		<Switch>
	// 			{routes.map((route, index) => {
	// 				if (route.isPublic) {
	// 					return <Route key={index} exact={route.isExact} path={route.path} component={route.component} />
	// 				} else {
	// 					return <PrivateRoute path={route.path} exact={route.isExact} key={index}>{route.component}</PrivateRoute>
	// 				}
	// 			})}
	// 		</Switch>
	// 	</main>

	// 	</div>
	// );
}

export default App;
