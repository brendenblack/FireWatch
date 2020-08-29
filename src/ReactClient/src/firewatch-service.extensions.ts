import * as generated from './firewatch-service.g';
import { useAuthState } from "./authentication/AuthContext";
import AuthService from './authentication/authService';
import { env } from 'process';

export class AuthApiBase {
    protected transformOptions(options: RequestInit): Promise<RequestInit> {
        // const authState = useAuthState();
        const authService = new AuthService();
        return authService.getUser().then(user => {
            

            if (user) {
                options.headers = { ...options.headers, authorization: `Bearer ${user.access_token}` };
            } 

            return Promise.resolve(options);
        })
    }

    protected getBaseUrl(something: string, defaultUrl: string | undefined): string {
        return process.env.REACT_APP_API_URL ?? defaultUrl ?? something;
    }
}