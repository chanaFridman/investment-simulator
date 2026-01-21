import { useState, useEffect } from 'react';

export const useTimer = (targetDateIso: string): string => {
    const [timeLeft, setTimeLeft] = useState<number>(0);

    useEffect(() => {
        const target = new Date(targetDateIso).getTime();

        const updateTimer = () => {
            const now = new Date().getTime();
            const diff = Math.max(0, (target - now) / 1000);
            setTimeLeft(diff);
        };

        updateTimer();
        const interval = setInterval(updateTimer, 100);

        return () => clearInterval(interval);
    }, [targetDateIso]);

    return timeLeft.toFixed(1);
};