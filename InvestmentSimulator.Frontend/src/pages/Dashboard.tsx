import {  } from '../context/InvestmentContext';
import { useTimer } from '../hooks/useTimer';
import { InvestmentCard } from '../components/investment/InvestmentCard';
import { Header } from '../components/layout/Header';
import type { ActiveInvestment } from '../types';
import styles from './Dashboard.module.css';
import { useInvestmentSystem } from '../hooks/useInvestmentSystem';

const ActiveItem = ({ item }: { item: ActiveInvestment }) => {
    const timeLeft = useTimer(item.endsAtUtc);
    return (
        <li className={styles.activeItem}>
            <span>{item.investmentName}</span>
            <span className={styles.timer}>{timeLeft}s</span>
        </li>
    );
};

export const Dashboard = () => {
    const { user, options, activeInvestments, invest } = useInvestmentSystem();

    if (!user) return <div>Please Log in</div>;

    return (
        <div className={styles.dashboard}>
            <Header userName={user.name} balance={user.balance} />

            <main>
                <section>
                    <h2>Available Investments</h2>
                    <div className={styles.grid}>
                        {options.map((opt, idx) => (
                            <InvestmentCard 
                                key={`${opt.name}-${idx}`}
                                option={opt}
                                isActive={activeInvestments.some(i => i.investmentName === opt.name)}
                                canAfford={user.balance >= opt.requiredAmount}
                                onInvest={() => invest(opt)}
                            />
                        ))}
                    </div>
                </section>

                <section className={styles.portfolio}>
                    <h2>Active Portfolio</h2>
                    {activeInvestments.length === 0 ? <p>No active investments.</p> : (
                        <ul>
                            {activeInvestments.map(inv => (
                                <ActiveItem key={inv.id} item={inv} />
                            ))}
                        </ul>
                    )}
                </section>
            </main>
        </div>
    );
};