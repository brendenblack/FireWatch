import { RouteComponentProps, useParams } from "react-router-dom";
import React from "react";


interface JournalProps extends RouteComponentProps<any> {

}

interface JournalState {

}

export default class JournalPage extends React.Component<JournalProps, JournalState> {
    constructor(props: JournalProps) {
        super(props);

        this.accountId = +props.match.params["accountId"];
    }

    accountId: number;

    render() {
        console.log("Account ID", this.accountId);
        return <div>Journal page</div>
    }


}