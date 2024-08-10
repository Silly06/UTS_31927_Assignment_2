import axios from 'axios';

export class ApiService {
    private static backendUrl: string | undefined = import.meta.env.VITE_BACKEND_URL

    static getCompleteUrl(endpoint: string): string {
        if (!this.backendUrl) {
            throw new Error('Backend URL is not available.');
        }
        return `${this.backendUrl}/${endpoint}`;
    }

    static async get<T>(endpoint: string): Promise<T> {
        const url = this.getCompleteUrl(endpoint);
        const response = await axios.get<T>(url);
        return response.data;
    }

    static async post<T>(endpoint: string, data: any): Promise<T> {
        const url = this.getCompleteUrl(endpoint);
        const response = await axios.post<T>(url, data);
        return response.data;
    }
}