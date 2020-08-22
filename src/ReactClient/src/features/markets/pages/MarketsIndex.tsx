import { RouteComponentProps, BrowserRouter, Switch, Route, NavLink } from "react-router-dom";
import React from "react";
import JournalPage from "./Journal";
import AccountsPage from "./Accounts";


interface MarketsIndexProps extends RouteComponentProps<any> {

}

interface MarketsIndexState {

}

export default class MarketsIndexPage extends React.Component<MarketsIndexProps, MarketsIndexState> {

    render() {
        return (
            <div className="flex">
                <div className="h-screen bg-gray-700 w-1/5 p-6">
                    <NavLink to="accounts">Accounts</NavLink>
                </div>
                <div className="bg-gray-500 flex-grow p-6">
                    <BrowserRouter>
                        <Switch>
                            <Route path="/" exact component={AccountsPage}  />
                            <Route path="accounts" component={AccountsPage} />
                            <Route path=":accountId/journal" component={JournalPage} />
                        </Switch>
                    </BrowserRouter>
                </div>
            </div>);
    }


}