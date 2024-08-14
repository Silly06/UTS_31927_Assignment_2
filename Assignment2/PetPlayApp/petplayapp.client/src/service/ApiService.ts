import axios from 'axios';

export class ApiService {
    private static backendUrl: string | undefined = import.meta.env.VITE_BACKEND_URL

    static getCompleteUrl(endpoint: string): string {
        if (!this.backendUrl) {
            throw new Error('Backend URL is not available.');
        }
        return `${this.backendUrl}/${endpoint}`;
    }
}