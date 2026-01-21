import React from 'react';
import { Card } from '../common/Card';
import { Button } from '../common/Button';
import type { InvestmentOption } from '../../types';
import styles from './InvestmentCard.module.css'; 

interface Props {
    option: InvestmentOption;
    onInvest: () => void;
    isActive: boolean;
    canAfford: boolean;
}

const InvestmentCardComponent: React.FC<Props> = ({ option, onInvest, isActive, canAfford }) => {
    const handleInvestClick = () => {
        if (!isActive && canAfford) {
            onInvest();
        }
    };

    return (
        <Card title={option.name} className={styles.investmentCard}>
            
            <div className={styles.stats}>
                <p>Cost: <span>${option.requiredAmount}</span></p>
                <p>Return: <span className={styles.profit}>${option.expectedReturn}</span></p>
                <p>Duration: {option.durationSeconds}s</p>
            </div>

            <div className={styles.actions}>
                <Button 
                    variant={isActive ? 'outline' : 'primary'}
                    onClick={handleInvestClick}
                    disabled={isActive || !canAfford}
                    isLoading={isActive}
                    className={styles.fullWidth} 
                >
                    {isActive ? 'Running...' : canAfford ? 'Invest Now' : 'Insufficient Funds'}
                </Button>
            </div>
        </Card>
    );
};

export const InvestmentCard = React.memo(InvestmentCardComponent);