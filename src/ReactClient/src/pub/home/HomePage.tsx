import React from "react";

export class HomePage extends React.Component {

    render() {
        return (
            <div className="bg-gray-200 flex items-center justify-center h-screen flex-col">
                <h1 className="font-sans text-6xl text-gray-800">FIREwatch</h1>
                <h2 className="font-sans text-lg text-gray-700">Financial Independence, Retire Early</h2>
            </div>
        );
    }
}