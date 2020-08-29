import React from 'react';
import { Link } from 'react-router-dom';
import cx from "classnames"
import routes from './routes';
import { useAuthState } from '../authentication/AuthContext';

export function Navbar() {
    const [ isOpen, setOpen] = React.useState<boolean>(false);
    const auth = useAuthState();

    return (
        <header className="bg-gray-900 sm:flex sm:justify-between sm:px-4 sm:py-3 sm:items-center">
            <div className="flex items-center justify-between px-4 py-3 sm:p-0">
                <div>
                    <h1 className="font-sans text-xl text-gray-300">Firewatch</h1>
                </div>
                <div className="sm:hidden">
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
            
               {!auth.isAuthenticated &&
                <div className={`${cx({ "block": isOpen, "hidden": !isOpen })} px-2 pt-2 pb-4 sm:flex sm:p-0`}>
                        <Link to="/login" className="block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1">
                            Login
                        </Link>
                        <Link to="/register" className="mt-1 block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1 sm:mt-0 sm:ml-2">
                            Register
                        </Link>
                        <Link to="/demo" className="mt-1 block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1 sm:mt-0 sm:ml-2">
                            Demo
                        </Link>
                    </div>}
                    
                {auth.isAuthenticated && 
                    <div className={`${cx({ "block": isOpen, "hidden": !isOpen })} px-2 pt-2 pb-4 sm:flex`}>
                        <Link to="/en/dashboard" className="block text-white font-semibold rounded hover:bg-gray-800 px-2 py-1">Dashboard</Link>
                    </div>}
        </header>);
}
