# Investment Simulator

Full-stack investment account simulator with real-time updates using SignalR and event-driven architecture.

---

## Tech Stack

| Layer | Technology |
|-------|-----------|
| **Frontend** | React + TypeScript + Vite |
| **Backend** | ASP.NET Core 8 Web API |
| **Real-time** | SignalR |
| **Background** | Channel Queue + BackgroundService |

---

## Prerequisites

- Node.js 18+
- .NET 8 SDK

---

## Quick Start

### Backend
```bash
cd InvestmentSimulator.Backend
dotnet restore
dotnet run
```
http://localhost:5011 | Swagger: http://localhost:5011/swagger

### Frontend
```bash
cd InvestmentSimulator.Frontend
npm install
npm run dev
```
http://localhost:5173

---

## Features

**Page 1 - Login**
- Username validation (3-100 chars, English letters only)
- Frontend + Backend validation

**Page 2 - Dashboard**
- Real-time balance display
- Active investments with countdown timers
- 3 investment options:
  - Short-term: $10 → $20 (10 sec)
  - Mid-term: $100 → $250 (30 sec)
  - Long-term: $1,000 → $3,000 (60 sec)

---

## Architecture

**Event-Driven Flow:**
```
Invest Button → API (202 Accepted) → Channel Queue → 
Background Service → Timer → Balance Update → SignalR Broadcast
```

**Benefits:** Non-blocking, real-time, scalable

---

## 🔌 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/user/login` | Login |
| `GET` | `/api/investment/options` | Available investments |
| `POST` | `/api/investment/invest` | Create investment |
| `GET` | `/api/investment/active` | Active investments |

---
