# TalentSync

TalentSync is a production-oriented **Recruitment Management Platform** built with **ASP.NET Core Web API** and **React**, following **Clean Architecture** principles.

The platform is designed to demonstrate enterprise-level software engineering practices including scalable architecture, secure authentication, role-based authorization, recruitment workflows, interview management, real-time communication, structured API design, pagination, filtering, and a modern feature-based frontend.

> **Project Status:** 🚧 Active Development

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
  - [Authentication & Authorization](#authentication--authorization)
  - [User Management](#user-management)
  - [Job Management](#job-management)
  - [Candidate Management](#candidate-management)
  - [Application Management](#application-management)
  - [Screening & Recruitment Workflow](#screening--recruitment-workflow)
  - [Interview Management](#interview-management)
  - [Dashboards](#dashboards)
  - [Notification System](#notification-system)
  - [File Management](#file-management)
  - [Human Resource Management](#human-resource-management)
- [Pagination & Filtering](#pagination--filtering)
- [Architecture](#architecture)
- [Design Patterns & Engineering Practices](#design-patterns--engineering-practices)
- [Technology Stack](#technology-stack)
- [Project Structure](#project-structure)
- [Frontend Architecture](#frontend-architecture)
- [Database](#database)
- [Configuration](#configuration)
- [Getting Started](#getting-started)
- [Running with Docker](#running-with-docker)
- [Running Without Docker](#running-without-docker)
- [Running the Frontend](#running-the-frontend)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [DevOps](#devops)
- [Current Modules](#current-modules)
- [Roadmap](#roadmap)
- [Contributing](#contributing)
- [License](#license)

---

# Overview

TalentSync is designed as a modular recruitment and HR platform that connects candidates, recruiters, HR professionals, and managers through a centralized recruitment workflow.

The system covers the recruitment lifecycle from:

```text
Candidate
    │
    ▼
Job Discovery
    │
    ▼
Application
    │
    ▼
Screening
    │
    ▼
Interview Scheduling
    │
    ▼
Interview Outcome
    │
    ▼
Selection / Rejection
```

The backend is implemented using ASP.NET Core and follows Clean Architecture to maintain separation between business rules, application services, persistence, and presentation.

The frontend uses React + TypeScript with a feature-based architecture designed for maintainability and scalability.

---

# Features

## Authentication & Authorization

- User Registration
- User Login
- JWT Authentication
- Refresh Token Authentication
- Role-Based Authorization
- Protected API Endpoints
- Secure Password Hashing
- Role-Based Access Control

---

## User Management

- User Management
- Role Management
- User Role Assignment
- Profile Management
- User Authentication
- Role-based access to application features

---

## Job Management

- Job Creation
- Job Management
- Job Details
- Job Status Management
- Open Job Listing
- Candidate Job Portal
- Job Search
- Job Filtering
- Job Pagination
- Recruiter/HR Job Management

---

## Candidate Management

- Candidate Management
- Candidate Profiles
- Candidate Tracking
- Candidate Job Portal
- Candidate Application Tracking
- Resume Management
- Candidate Dashboard

---

## Application Management

- Job Applications
- Application Details
- Application Status Management
- Application Tracking
- Application Pagination
- Application Filtering
- Candidate Application History
- Recruitment Workflow Integration

---

## Screening & Recruitment Workflow

TalentSync supports a structured recruitment workflow.

Typical flow:

```text
Application Submitted
        │
        ▼
     Screening
        │
   ┌────┴────┐
   │         │
 Failed    Passed
   │         │
   ▼         ▼
Rejected   Interview
             │
             ▼
       Interview Outcome
             │
        ┌────┴────┐
        │         │
      Passed    Failed
        │         │
        ▼         ▼
    Selection  Rejected
```

The system validates application status transitions before allowing workflow operations.

---

# Interview Management

TalentSync includes an interview management workflow for recruiters, HR users, and managers.

### Interview capabilities

- Interview Scheduling
- Interview Assignment
- Interview Rescheduling
- Interview Cancellation
- Interview Outcome Recording
- Interview Status Management
- Interview Details
- Interview Search
- Interview Filtering
- Interview Date Range Filtering
- Interview Pagination
- Upcoming Interviews
- Today's Interviews
- Completed Interviews
- Manager Assigned Interviews

### Interview statuses

```text
Scheduled
Completed
Passed
Failed
Cancelled
```

### Interview scheduling

Interview scheduling includes validation around:

- Application state
- Screening result
- Interviewer assignment
- Interviewer role
- Existing interview state
- Scheduled interview time
- Transactional persistence

---

# Dashboards

TalentSync provides role-specific dashboards.

## Candidate Dashboard

Provides candidates with recruitment-related information including:

- Profile Completion
- Application Statistics
- Recent Applications
- Upcoming Interviews
- Quick Actions

---

## Recruiter Dashboard

Provides recruitment-focused insights including:

- Recruitment Statistics
- Recent Applications
- Recent Jobs
- Upcoming Interviews
- Job and application information

---

## HR Dashboard

The HR dashboard provides an overview of recruitment operations.

### Summary

- Total Jobs
- Open Jobs
- Total Candidates
- Total Applications
- Today's Interviews

### Dashboard sections

- Recent Applications
- Recent Jobs
- Upcoming Interviews

---

## Manager Dashboard

The Manager dashboard provides manager-specific recruitment insights.

### Summary

- Open Jobs
- Total Applications
- Pending Screenings
- Today's Interviews
- Upcoming Interviews
- Completed Interviews

### Dashboard sections

- Upcoming Interviews
- Recent Applications
- Recent Jobs

Manager-specific interview data is scoped to the manager/interviewer assignment where applicable.

---

# Notification System

TalentSync includes a real-time notification infrastructure.

- In-App Notifications
- SignalR Real-Time Notifications
- Read / Unread Notifications
- Unread Notification Counter
- Notification Management
- Real-Time Client Updates

The frontend integrates SignalR to receive notifications without requiring manual polling.

---

# File Management

Resume and file management functionality includes:

- Resume Upload
- Resume Download
- Resume Replacement
- File Validation
- Cloudinary Integration

Sensitive storage credentials are kept outside source control.

---

# Human Resource Management

The HR module provides foundational employee-management capabilities.

- Employee Management
- Employee Records
- Employee Directory
- Employee Onboarding

Additional HR functionality is planned for future releases.

---

# Pagination & Filtering

TalentSync uses reusable pagination and filtering patterns across recruitment modules.

### Pagination capabilities

- Page Number
- Page Size
- Total Records
- Paginated Data
- Reusable Pagination Response

Example:

```json
{
  "pageNumber": 1,
  "pageSize": 10,
  "totalRecords": 50,
  "data": []
}
```

### Filtering capabilities

Depending on the module, filters can include:

- Search
- Status
- From Date
- To Date
- Role-specific filtering

Query filters are centralized where appropriate using reusable query extensions.

---

# Architecture

TalentSync follows **Clean Architecture**.

```text
┌──────────────────────────────┐
│       TalentSync.Api         │
│        Presentation          │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│   TalentSync.Application     │
│       Business Logic         │
│   DTOs / Services / Rules    │
└──────────────┬───────────────┘
               │
               ▼
┌──────────────────────────────┐
│      TalentSync.Domain       │
│ Entities / Enums / Rules     │
└──────────────┬───────────────┘
               ▲
               │
┌──────────────┴───────────────┐
│   TalentSync.Infrastructure  │
│ Database / Repositories /    │
│ External Services            │
└──────────────────────────────┘
```

### Layer responsibilities

#### API

Responsible for:

- HTTP endpoints
- Controllers
- Authentication configuration
- Middleware
- API request/response handling
- Dependency injection composition

#### Application

Responsible for:

- Business use cases
- Services
- DTOs
- Interfaces
- Validation
- Application-level business rules

#### Domain

Responsible for:

- Core entities
- Domain enums
- Core business concepts
- Domain-independent logic

#### Infrastructure

Responsible for:

- Entity Framework Core
- SQL Server
- Repositories
- Unit of Work
- External services
- Cloudinary integration
- Persistence configuration

---

# Design Patterns & Engineering Practices

TalentSync uses several enterprise-oriented patterns and engineering practices.

### Architecture & Design

- Clean Architecture
- Feature-Based Frontend Architecture
- Separation of Concerns
- SOLID Principles

### Backend Patterns

- Repository Pattern
- Unit of Work Pattern
- Service Layer Pattern
- Dependency Injection
- DTO Pattern
- Strategy Pattern
- Options Pattern

### Data Access

- Entity Framework Core
- AsNoTracking for read operations
- Query composition
- Reusable query extensions
- Pagination
- Filtering
- EF Core migrations

### API & Application

- RESTful API design
- Async/Await
- CancellationToken support
- Global Exception Handling
- Structured Logging
- DTO-based API contracts
- AutoMapper

### Security

- JWT authentication
- Refresh tokens
- Role-based authorization
- Password hashing
- Protected endpoints
- Secret configuration outside source control

---

# Technology Stack

## Backend

- ASP.NET Core Web API
- .NET 10
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- Refresh Tokens
- SignalR
- AutoMapper
- Cloudinary
- Dependency Injection

---

## Frontend

- React
- TypeScript
- Vite
- Tailwind CSS
- React Router
- Axios
- Custom React Hooks
- Feature-Based Architecture
- Shared Components

---

## Database

- SQL Server
- Entity Framework Core
- Code First
- EF Core Migrations

---

## Testing

- xUnit
- Moq
- FluentAssertions
- Coverlet

---

## DevOps

- Docker
- Docker Compose

---

# Project Structure

```text
TalentSync
│
├── TalentSync.Api
│   ├── Controllers
│   ├── Middleware
│   ├── Extensions
│   └── Configuration
│
├── TalentSync.Application
│   ├── Common
│   │   └── Pagination
│   ├── DTOs
│   ├── Interfaces
│   │   ├── Repositories
│   │   └── Services
│   └── Services 
│
├── TalentSync.Domain
│   ├── Entities
│   ├── Enums
│   └── Common
│
├── TalentSync.Infrastructure
│   ├── Persistence
│   ├── Repositories
│   ├── Services
│   └── Configurations
│
├── TalentSync.Tests
│   └── Services
│
└── frontend
    └── src
```

---

# Frontend Architecture

The React frontend follows a feature-oriented architecture.

```text
frontend/
│
└── src/
    │
    ├── features/
    │   │
    │   ├── auth/
    │   ├── applications/
    │   ├── candidates/
    │   ├── dashboard/
    │   ├── employees/
    │   ├── interviews/
    │   ├── jobs/
    │   ├── notifications/
    │   ├── profile/
    │   ├── recruiters/
    │   └── hr/
    │
    ├── shared/
    │   ├── api/
    │   ├── components/
    │   ├── hooks/
    │   ├── types/
    │   └── utils/
    │
    └── App.tsx
```

The frontend architecture focuses on:

- Feature isolation
- Reusable shared components
- API abstraction
- Custom hooks
- Type-safe API contracts
- Role-specific pages
- Shared pagination components
- Shared UI components

---

# Database

TalentSync uses SQL Server with Entity Framework Core.

The application follows a Code First approach.

Major domain areas include:

- Users
- Roles
- User Roles
- Jobs
- Applications
- Candidates
- Interviews
- Notifications
- Employees
- Resumes

---

# Configuration

Sensitive configuration should not be committed to source control.

TalentSync supports ASP.NET Core User Secrets and environment-specific configuration.

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

> Never commit database passwords, JWT secrets, Cloudinary credentials, API keys, or other sensitive configuration to Git.

---

# Getting Started

## Prerequisites

### Recommended

- Docker Desktop

### Local Development

- .NET 10 SDK
- SQL Server
- Node.js
- npm

---

# Running with Docker

Clone the repository:

```bash
git clone https://github.com/Harshitrajpurohit/talentsync.git
```

Navigate into the repository:

```bash
cd talentsync
```

Start the application:

```bash
docker compose up --build
```

API:

```text
http://localhost:5000
```

Swagger:

```text
http://localhost:5000/swagger
```

---

# Running Without Docker

## Restore backend dependencies

```bash
dotnet restore
```

## Apply database migrations

```bash
dotnet ef database update
```

## Run the API

```bash
dotnet run --project TalentSync.Api
```

---

# Running the Frontend

Navigate to the frontend:

```bash
cd frontend
```

Install dependencies:

```bash
npm install
```

Start the development server:

```bash
npm run dev
```

The Vite development server will provide the frontend URL in the terminal.

---

# API Documentation

Swagger is available during development at:

```text
http://localhost:5000/swagger
```

The API is organized around resource-oriented endpoints covering:

- Authentication
- Users
- Roles
- Jobs
- Candidates
- Applications
- Interviews
- Notifications
- Dashboards
- Employees

---

# Testing  (because of change is many Endpoints some test cases will throw exception.)

TalentSync uses:

- xUnit
- Moq
- FluentAssertions
- Coverlet

Run all tests:

```bash
dotnet test
```

Current testing status:

- ✅ 180+ unit tests
- ✅ Service layer testing
- ✅ All current tests passing

Testing coverage will continue to expand as additional modules are implemented.

---

# DevOps

Current infrastructure includes:

- Docker
- Docker Compose
- Containerized SQL Server
- Environment-based configuration

The DevOps pipeline will continue to evolve as the project moves toward production deployment.

---

# Current Modules

The following modules are currently implemented or actively under development:

- Authentication
- Authorization
- User Management
- Role Management
- Profile Management
- Job Management
- Candidate Portal
- Candidate Management
- Resume Management
- Application Management
- Screening
- Interview Management
- Notifications
- Employee Management
- Candidate Dashboard
- Recruiter Dashboard
- HR Dashboard
- Manager Dashboard
- Pagination
- Search & Filtering

---

# Roadmap

The following capabilities are planned for future development.

## Recruitment

- Advanced Reports & Analytics
- Recruitment Analytics
- Candidate Matching
- Interview Analytics
- Recruitment Performance Metrics

## Communication

- Email Notifications
- Email Templates
- Automated Recruitment Emails

## Infrastructure

- Health Checks
- Background Jobs with Hangfire
- Redis Caching
- API Rate Limiting
- GitHub Actions CI/CD
- Azure Deployment
- Serilog
- Application Monitoring
- Distributed Tracing

## Testing

- Integration Testing
- API Testing
- End-to-End Testing
- Increased Automated Test Coverage

## HR Management

- Leave Management
- Attendance Management
- Payroll Module

## Enterprise Features

- Audit Logging
- Advanced Permissions
- Organization Management
- Multi-Tenant Support

---

# Development Workflow

Feature development follows a feature-branch and pull-request workflow.

Example:

```bash
git switch -c feature/<feature-name>
```

After completing the implementation:

```bash
git add .
git commit -m "feat: <description>"
git push -u origin feature/<feature-name>
```

Create a pull request against the appropriate development branch.

---

# Contributing

Contributions, issues, and feature requests are welcome.

For significant changes:

1. Create a feature branch.
2. Implement the feature.
3. Add or update tests.
4. Run the test suite.
5. Verify the API and frontend.
6. Commit the changes.
7. Push the feature branch.
8. Open a pull request.

---

# License

This project is developed for **learning, portfolio, and educational purposes**.

---

# Author

**Harshit Rajpurohit**

B.Tech Computer Science Engineering

GitHub:  
https://github.com/Harshitrajpurohit

Portfolio:  
https://harshit-rajpurohit.vercel.app
