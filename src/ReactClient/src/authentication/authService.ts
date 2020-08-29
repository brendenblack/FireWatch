import { UserManager, WebStorageStateStore, Log } from "oidc-client";
import { IDENTITY_CONFIG, METADATA_OIDC } from "./authConstants";

export default class AuthService {
    UserManager: UserManager;

    constructor() {
        this.UserManager = new UserManager({
            ...IDENTITY_CONFIG,
            userStore: new WebStorageStateStore({ store: window.sessionStorage }),
            metadata: { ...METADATA_OIDC }
        });

        Log.logger = console;
        Log.level = Log.INFO;
        
        this.UserManager.events.addUserLoaded((user) => {
            if (window.location.href.indexOf("signin-oidc") !== -1) {
                this.navigateToScreen();
            }
        });

        this.UserManager.events.addSilentRenewError((e) => {
            console.log("silent renew error", e.message);
        });

        this.UserManager.events.addAccessTokenExpired(() => {
            console.log("token expired");
            this.logout()
        });

        // this.UserManager.events.addUserSignedOut(() => {

        // })
    }

    signinRedirectCallback = () => {
        
        this.UserManager.signinRedirectCallback().then(() => {
            "";
        });
    };

    getUser = async () => {
        console.log('AuthService#getUser');
        const user = await this.UserManager.getUser();
        // if (!user) {
        //     return await this.UserManager.signinRedirectCallback();
        // }
        return user;
    };

    parseJwt = (token: string) => {
        const base64Url = token.split(".")[1];
        const base64 = base64Url.replace("-", "+").replace("_", "/");
        return JSON.parse(window.atob(base64));
    };


    signinRedirect = () => {
        console.log(`Setting 'redirectUri' to ${window.location.pathname}`);
        localStorage.setItem("redirectUri", window.location.pathname);

        this.UserManager.signinRedirect({});
    };


    navigateToScreen = () => {
        window.location.replace("/");
    };


    isAuthenticated = () => {
        const oidcStorage = JSON.parse(sessionStorage.getItem(`oidc.user:${process.env.REACT_APP_AUTH_URL}:${process.env.REACT_APP_IDENTITY_CLIENT_ID}`) || '{}')

        return (!!oidcStorage && !!oidcStorage.access_token)
    };

    isSigningIn: boolean = false;

    signinSilent = () => {
        if (!this.isSigningIn) {
            this.isSigningIn = true;
        this.UserManager.signinSilent()
            .then((user) => {
                console.log("signed in", user);
            })
            .catch((err) => {
                'An error occurred attempting to sign in silently';
                console.log(err);
            })
            .finally(() => { this.isSigningIn = false });
        }
    };

    signinSilentCallback = () => {
        this.UserManager.signinSilentCallback();
    };

    createSigninRequest = () => {
        return this.UserManager.createSigninRequest();
    };

    logout = () => {
        this.UserManager.signoutRedirect({
            id_token_hint: localStorage.getItem("id_token")
        });
        this.UserManager.clearStaleState();
    };

    signoutRedirectCallback = () => {
        this.UserManager.signoutRedirectCallback().then(() => {
            localStorage.clear();
            window.location.replace(process.env.REACT_APP_PUBLIC_URL || '/');
        });

        this.UserManager.clearStaleState();
    };
}