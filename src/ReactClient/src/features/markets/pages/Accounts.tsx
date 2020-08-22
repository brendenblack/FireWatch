import { RouteComponentProps, NavLink } from "react-router-dom";
import React from "react";
import { AccountDto } from "../../../firewatch-service.g";



interface AccountsProps extends RouteComponentProps<any> {

}

interface AccountsState {

}

export default class AccountsPage extends React.Component<AccountsProps, AccountsState> {

    accounts: AccountDto[] = [];
    
    componentDidMount() {
        // const client = new AccountsClient();

        // client.getAccounts().then(vm => {
        //     this.accounts = vm.accounts ?? [];
        // });
    }

    render() {
        return (
            <div>
                <h1>Accounts page</h1>
                <NavLink to={`${this.props.match.url}/1/journal`}>Account 1</NavLink>
            </div>);
    }


}