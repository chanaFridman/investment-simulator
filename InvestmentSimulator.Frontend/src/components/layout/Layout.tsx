import React from 'react';
import { Outlet } from 'react-router-dom';
import styles from './Layout.module.css';

export const Layout: React.FC = () => {
    return (
        <div className={styles.appContainer}>
            <main className={styles.mainContent}>
                <Outlet />
            </main>
            <footer className={styles.footer}>
                © 2026 Investment Simulator | Enterprise Edition
            </footer>
        </div>
    );
};