export interface User {
    userId: string;
    name: string;
    balance: number;
}

export interface InvestmentOption {
    name: string;
    requiredAmount: number;
    expectedReturn: number;
    durationSeconds: number;
}

export interface ActiveInvestment {
    id: string;
    userId: string;
    investmentName: string;
    amountInvested: number;
    expectedReturn: number;
    startedAtUtc: string; 
    endsAtUtc: string;    
}