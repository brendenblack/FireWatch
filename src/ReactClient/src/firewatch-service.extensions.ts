import * as generated from './firewatch-service.g';
import { useAuthState } from "./authorization/AuthContext";

export class AuthApiBase {
    protected transformOptions(options: RequestInit): Promise<RequestInit> {
        const token = useAuthState().user?.access_token
        if (token) {
            options.headers = { ...options.headers, authorization: `Bearer ${token}` }
        }

        return Promise.resolve(options);
    }
}