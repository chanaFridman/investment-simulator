import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { InvestmentProvider } from './context/InvestmentProvider';
import { useInvestmentSystem } from './hooks/useInvestmentSystem';
import { Layout } from './components/layout/Layout';
import { Dashboard } from './pages/Dashboard';
import { LoginPage } from './pages/LoginPage';
import type { JSX } from 'react';

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {
    const { user } = useInvestmentSystem();
    if (!user) {
        return <Navigate to="/" replace />;
    }
    return children;
};

function App() {
    return (
        <InvestmentProvider>
            <BrowserRouter>
                <Routes>
                    <Route element={<Layout />}>                   
                        <Route path="/" element={<LoginPage />} />                        
                        <Route 
                            path="/dashboard" 
                            element={
                                <ProtectedRoute>
                                    <Dashboard />
                                </ProtectedRoute>
                            } 
                        />
                        <Route path="*" element={<Navigate to="/" replace />} />
                    </Route>
                </Routes>
            </BrowserRouter>
        </InvestmentProvider>
    );
}

export default App;