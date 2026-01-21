import { createContext } from 'react';
import type { User, ActiveInvestment, InvestmentOption } from '../types';

export interface InvestmentContextType {
    user: User | null;
    options: InvestmentOption[];
    activeInvestments: ActiveInvestment[];
    isLoading: boolean;
    login: (name: string) => Promise<void>;
    invest: (option: InvestmentOption) => Promise<void>;
    error: string | null;
}

export const InvestmentContext = createContext<InvestmentContextType | undefined>(undefined);