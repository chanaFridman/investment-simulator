import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom'; 
import styles from './LoginPage.module.css'; 
import { useInvestmentSystem } from '../hooks/useInvestmentSystem';

export const LoginPage = () => {
    const { login, error: contextError, isLoading } = useInvestmentSystem();
    const [name, setName] = useState('');
    const [localError, setLocalError] = useState('');
    
    const navigate = useNavigate();

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLocalError('');

        if (name.length < 3) {
            setLocalError('Name must be at least 3 characters.');
            return;
        }

        if (!/^[a-zA-Z]+$/.test(name)) {
            setLocalError('Name must contain only English letters.');
            return;
        }

        try {
            await login(name);
            navigate('/dashboard');
        } catch (error) {
            console.error('Login error:', error);
        }
    };

    return (
        <div className={styles.loginContainer}>
            <div className={styles.loginCard}>
                <h1>Investment Simulator</h1>
                <p>Enter your name to start trading</p>
                
                <form onSubmit={handleSubmit}>
                    <input
                        type="text"
                        placeholder="Username (English letters only)"
                        value={name}
                        onChange={(e) => setName(e.target.value)}
                        className={styles.input}
                        maxLength={20}
                    />
                    
                    {(localError || contextError) && (
                        <div className={styles.error}>
                            {localError || contextError}
                        </div>
                    )}

                    <button 
                        type="submit" 
                        disabled={isLoading}
                        className={styles.btnLogin}
                    >
                        {isLoading ? 'Connecting...' : 'Login'}
                    </button>
                </form>
            </div>
        </div>
    );
};