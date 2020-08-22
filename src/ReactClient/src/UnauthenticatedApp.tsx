import React from 'react';
import { HomePage } from './pub/home/HomePage';
import { Link, Switch, Route } from 'react-router-dom';
import cx from "classnames"
import LoginPage from './pub/LoginPage';
import DemoPage from './pub/DemoPage';

function UnauthenticatedApp() {
    const [ isOpen, setOpen] = React.useState<boolean>(false);

    return (
        <div>
            <header className="bg-gray-900">
				<div className="flex items-center justify-between px-4 py-3">
					<div>
						<h1 className="font-sans text-xl text-gray-300">Firewatch</h1>
					</div>
					<div>
						<button type="button" className="text-gray-500 block focus:text-white hover:text-white" onClick={() => setOpen(!isOpen)}>
							<svg className={`${cx({ "hidden": !isOpen })} h-6 w-6 fill-current`} viewBox="0 0 24 24">
								<path fillRule="evenodd" d="M18.278 16.864a1 1 0 0 1-1.414 1.414l-4.829-4.828-4.828 4.828a1 1 0 0 1-1.414-1.414l4.828-4.829-4.828-4.828a1 1 0 0 1 1.414-1.414l4.829 4.828 4.828-4.828a1 1 0 1 1 1.414 1.414l-4.828 4.829 4.828 4.828z"/>
							</svg>
							<svg className={`${cx({ "hidden": isOpen })} h-6 w-6 fill-current`} viewBox="0 0 24 24">
								<path fillRule="evenodd" d="M4 5h16a1 1 0 0 1 0 2H4a1 1 0 1 1 0-2zm0 6h16a1 1 0 0 1 0 2H4a1 1 0 0 1 0-2zm0 6h16a1 1 0 0 1 0 2H4a1 1 0 0 1 0-2z"/>
							</svg>
						</button>
					</div>
				</div>
				<div className={`${cx({ "block": isOpen, "hidden": !isOpen })} px-2 pt-2 pb-4`}>
					<Link to="/login" className="block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1">
                        Login
                    </Link>
                    <Link to="/register" className="mt-1 block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1">
                        Register
                    </Link>
                    <Link to="/demo" className="mt-1 block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1">
                        Demo
                    </Link>
				</div>
			</header>
            <div>
                <Switch>
                    <Route path="/" exact component={HomePage} />
                    <Route path="/login" component={LoginPage} />
                    {/* <Route path="/demo" component={DemoPage} /> */}
                </Switch>
            </div>
        </div>
    );
}

export default UnauthenticatedApp;