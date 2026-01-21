import React from 'react';
import styles from './Button.module.css';

interface ButtonProps extends React.ButtonHTMLAttributes<HTMLButtonElement> {
    variant?: 'primary' | 'danger' | 'outline';
    isLoading?: boolean;
}

export const Button: React.FC<ButtonProps> = ({ 
    children, 
    variant = 'primary', 
    isLoading = false, 
    disabled, 
    className = '',
    ...props 
}) => {
    const combinedClassName = `
        ${styles.btn} 
        ${styles[variant]} 
        ${className}
    `.trim();

    return (
        <button
            disabled={disabled || isLoading}
            className={combinedClassName}
            {...props}
        >
            {isLoading && <span className={styles.spinner} />}
            {children}
        </button>
    );
};