import { useContext } from 'react';
import { InvestmentContext } from '../context/InvestmentContext'; 

export const useInvestmentSystem = () => {
    const context = useContext(InvestmentContext);
    if (!context) {
        throw new Error("useInvestmentSystem must be used within InvestmentProvider");
    }
    return context;
};