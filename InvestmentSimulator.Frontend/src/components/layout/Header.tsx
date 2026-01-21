import React from 'react';
import styles from './Header.module.css';

interface HeaderProps {
    userName: string;
    balance: number;
}

export const Header: React.FC<HeaderProps> = ({ userName, balance }) => {
    return (
        <header className={styles.header}>
            <h1 className={styles.title}>
                Welcome, {userName} 
            </h1>
            
            <div className={styles.balanceContainer}>
                <span className={styles.balanceLabel}>Balance:</span>
                <span>${balance.toLocaleString()}</span>
            </div>
        </header>
    );
};