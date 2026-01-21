import axios from 'axios';
import type { User, InvestmentOption, ActiveInvestment } from '../types';

const BASE_URL = 'http://localhost:5011/api';

export const apiClient = axios.create({
    baseURL: BASE_URL,
    headers: { 'Content-Type': 'application/json' }
});

export const api = {
    login: async (name: string): Promise<User> => 
        (await apiClient.post<User>('/User/login', { name })).data,

    getOptions: async (): Promise<InvestmentOption[]> => 
        (await apiClient.get<InvestmentOption[]>('/Investment/options')).data,

    getActiveInvestments: async (userId: string): Promise<ActiveInvestment[]> => 
        (await apiClient.get<ActiveInvestment[]>(`/Investment/active/${userId}`)).data,

    invest: async (userId: string, investmentName: string): Promise<void> => {
        await apiClient.post('/Investment/invest', { userId, investmentName });
    }
};