import { Component } from "react";
import { LoginPage } from "../pub/LoginPage";

class AppRoute {
    constructor(display: string, path: string, component: any, isSecure = true, isDisplayed = false) {
        this.display = display;
        this.path = path;
        this.component = component;
        this.isSecure = isSecure;
        this.isDisplayed = isDisplayed;
    }

    readonly display: string;
    readonly path: string;
    readonly component: Component;

    readonly isSecure: boolean;
    readonly isDisplayed: boolean;
}


const routes: AppRoute[] = [
    new AppRoute('Login', '/login', LoginPage, false, true)
]

export default routes;