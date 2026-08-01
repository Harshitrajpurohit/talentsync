# TalentSync

TalentSync is a production-oriented **Recruitment Management Platform** built with **ASP.NET Core Web API** following **Clean Architecture** principles.

The project is designed to demonstrate enterprise-level backend development practices, scalable architecture, secure authentication, real-time communication, and a modern React frontend.

> **Project Status:** 🚧 Active Development

---

# Features

## Authentication & Authorization

- JWT Authentication
- Refresh Token Authentication
- Role-Based Authorization
- Secure Password Hashing
- Protected API Endpoints

---

## User Management

- User Registration & Login
- User Management
- Role Management
- User Role Assignment
- Profile Management

---

## Recruitment Management

- Job Management
- Candidate Job Portal
- Resume Upload & Replacement
- Job Applications
- Candidate Dashboard
- Recruiter Dashboard
- HR Dashboard
- Interview Scheduling
- Recruitment Workflow
- Job Status Management
- Candidate Tracking

---

## Human Resource Management

- Employee Management
- Employee Records
- Employee Directory
- Employee Onboarding

---

## Dashboard

Role-based dashboards for:

- HR
- Recruiter
- Candidate
- Admin
- Manager
- Employee

---

## Notification System

- In-App Notifications
- SignalR Real-Time Notifications
- Read / Unread Notifications
- Notification Counter

---

## File Management

- Resume Upload
- Resume Download
- Resume Replacement
- Cloudinary Integration

---

## Infrastructure

- Clean Architecture
- Repository Pattern
- Unit of Work Pattern
- Dependency Injection
- AutoMapper
- Entity Framework Core
- SQL Server
- Global Exception Handling
- Pagination
- Validation
- Logging
- CancellationToken Support

---

## Frontend

Built with:

- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- Axios
- Custom Hooks
- Feature-Based Architecture

---

## Testing

- xUnit
- Moq
- 180+ Unit Tests
- Service Layer Testing

---

## DevOps

- Docker
- Docker Compose

---

# Architecture

TalentSync follows **Clean Architecture**.

```
Presentation (API)
        │
Application
        │
Domain
        │
Infrastructure
```

---

# Design Patterns

- Clean Architecture
- Repository Pattern
- Unit of Work Pattern
- Service Layer Pattern
- Strategy Pattern
- Dependency Injection
- DTO Pattern
- AutoMapper
- Options Pattern

---

# Tech Stack

## Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Refresh Tokens
- SignalR
- AutoMapper
- Cloudinary

---

## Frontend

- React
- TypeScript
- Vite
- Tailwind CSS
- Axios
- React Router

---

## Database

- SQL Server
- Entity Framework Core

---

## Testing

- xUnit
- Moq
- FluentAssertions

---

## DevOps

- Docker
- Docker Compose

---

# Project Structure

```
TalentSync
│
├── TalentSync.Api
├── TalentSync.Application
├── TalentSync.Domain
├── TalentSync.Infrastructure
├── TalentSync.Tests
└── frontend (React)
```

---

# Configuration

Sensitive configuration is stored using **ASP.NET Core User Secrets**.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<your-connection-string>"
  },

  "JwtConfig": {
    "Key": "<jwt-secret>"
  },

  "AdminUser": {
    "Password": "<admin-password>"
  },

  "Cloudinary": {
    "CloudName": "<cloud-name>",
    "ApiKey": "<api-key>",
    "ApiSecret": "<api-secret>"
  }
}
```

---

# Getting Started

## Prerequisites

### Option 1 (Recommended)

- Docker Desktop

### Option 2

- .NET 10 SDK
- SQL Server
- Node.js (Frontend)

---

# Running with Docker

Clone the repository

```bash
git clone https://github.com/Harshitrajpurohit/talentsync.git
```

Navigate into the project

```bash
cd talentsync
```

Run

```bash
docker compose up --build
```

API

```
http://localhost:5000
```

Swagger

```
http://localhost:5000/swagger
```

---

# Running Without Docker

Restore packages

```bash
dotnet restore
```

Apply migrations

```bash
dotnet ef database update
```

Run API

```bash
dotnet run --project TalentSync.Api
```

---

## Frontend

Navigate to the frontend project

```bash
cd frontend
```

Install dependencies

```bash
npm install
```

Start development server

```bash
npm run dev
```

---

# Testing

Run all unit tests

```bash
dotnet test
```

Current Status

- ✅ 180+ Unit Tests
- ✅ All Tests Passing

---

# Current Modules

- Authentication
- User Management
- Role Management
- Profile Management
- Job Management
- Candidate Portal
- Resume Management
- Employee Management
- Notifications
- Candidate Dashboard
- Recruiter Dashboard
- HR Dashboard

---

# Upcoming Features

- Interview Management
- Reports & Analytics
- Email Notifications
- Health Checks
- Background Jobs (Hangfire)
- Redis Caching
- Rate Limiting
- GitHub Actions CI/CD
- Azure Deployment
- Serilog Logging
- Monitoring
- Integration Testing
- Leave Management
- Attendance Management
- Payroll Module
- Multi-Tenant Support

---

# Contributing

Contributions, issues, and feature requests are welcome.

---

# License

This project is developed for learning, portfolio, and educational purposes.