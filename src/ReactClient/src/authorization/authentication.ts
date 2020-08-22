import { User, UserManager, WebStorageStateStore } from 'oidc-client';

const config = {
    authority: "https://localhost:5001",
    client_id: "js",
    redirect_uri: "https://localhost:5003/callback.html",
    response_type: "code",
    scope:"openid profile api1",
    post_logout_redirect_uri : "https://localhost:5003/index.html",
}

const mgr = new UserManager(config);

export { config }