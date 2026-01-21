import React, { useState, useEffect, useCallback, useRef } from 'react';
import * as signalR from '@microsoft/signalr';
import axios from 'axios';
import type { User, ActiveInvestment, InvestmentOption } from '../types';
import { api } from '../api/client';
import { InvestmentContext } from './InvestmentContext'; 

export const InvestmentProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
    const [user, setUser] = useState<User | null>(null);
    const [options, setOptions] = useState<InvestmentOption[]>([]);
    const [activeInvestments, setActiveInvestments] = useState<ActiveInvestment[]>([]);
    const [connection, setConnection] = useState<signalR.HubConnection | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [isLoading, setIsLoading] = useState(false);

    const isConnecting = useRef(false);

    const fetchInitialData = useCallback(async (userId: string) => {
        try {
            const [opts, active] = await Promise.all([
                api.getOptions(),
                api.getActiveInvestments(userId)
            ]);
            setOptions(opts);
            setActiveInvestments(active);
            setError(null);
        } catch (err) {
            console.error("Failed to load data", err);
            setError("Failed to load investment data");
        }
    }, []);

    useEffect(() => {
        if (!user) return;
        
        const newConnection = new signalR.HubConnectionBuilder()
            .withUrl("http://localhost:5011/hubs/investmentHub")
            .withAutomaticReconnect()
            .build();
            
        setConnection(newConnection);
        
        return () => { 
            newConnection.stop().catch(err => console.error("Error stopping connection", err));
        };
    }, [user]);

    useEffect(() => {
        if (!connection || !user) return;

        const startConnection = async () => {
            if (connection.state !== signalR.HubConnectionState.Disconnected || isConnecting.current) {
                return; 
            }

            isConnecting.current = true;

            try {
                await connection.start();
                console.log("SignalR Connected!");
                await connection.invoke("JoinUserGroup", user.userId);

                connection.onreconnected(async () => {
                    try {
                        await connection.invoke("JoinUserGroup", user.userId);
                    } catch (e) {
                        console.error("Failed to re-join group after reconnect", e);
                    }
                });
                
                connection.on("BalanceUpdated", (newBalance: number) => {
                    setUser(prev => prev ? { ...prev, balance: newBalance } : null);
                });
                connection.on("InvestmentCreated", (investment: ActiveInvestment) => {
                    setActiveInvestments(prev => [...prev, investment]);
                });
                connection.on("InvestmentCompleted", (data: { investmentId: string }) => {
                    setActiveInvestments(prev => prev.filter(i => i.id !== data.investmentId));
                });
            } catch (err) {
                console.error("SignalR Connection Failed", err);
            } finally {
                isConnecting.current = false;
            }
        };

        startConnection();

        return () => {
            connection.off("BalanceUpdated");
            connection.off("InvestmentCreated");
            connection.off("InvestmentCompleted");
        };
    }, [connection, user]);

    const login = async (name: string) => {
        setIsLoading(true);
        setError(null);
        try {
            const userData = await api.login(name);
            setUser(userData);
            await fetchInitialData(userData.userId);
        } catch (err) {
            let errorMsg = "Login failed";
            if (axios.isAxiosError(err) && err.response?.data && typeof err.response.data === 'object') {
                const data = err.response.data as { message?: string; detail?: string };
                if (data.message) {
                    errorMsg = data.message;
                } else if (data.detail) {
                    errorMsg = data.detail;
                }
            }
            setError(errorMsg);
            throw err;
        } finally {
            setIsLoading(false);
        }
    };

    const invest = useCallback(async (option: InvestmentOption) => {
        if (!user) return;
        try {
            await api.invest(user.userId, option.name);
            setError(null);
        } catch (err) {
            let msg = "Investment failed";
            if (axios.isAxiosError(err) && err.response?.data && typeof err.response.data === 'object') {
                const data = err.response.data as { message?: string; detail?: string };
                if (data.message) {
                    msg = data.message;
                } else if (data.detail) {
                    msg = data.detail;
                }
            }
            setError(msg);
        }
    }, [user]);

    return (
        <InvestmentContext.Provider value={{ user, options, activeInvestments, isLoading, login, invest, error }}>
            {children}
        </InvestmentContext.Provider>
    );
};