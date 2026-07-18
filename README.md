# TalentSync

TalentSync is a production-oriented Human Resource Management System (HRMS) and Recruitment Platform built with **ASP.NET Core Web API**, following **Clean Architecture** and modern software engineering practices.

The project focuses on building a scalable, maintainable, and enterprise-ready recruitment management system with authentication, workflow management, real-time notifications, and containerized deployment.

> **Project Status:**  Active Development

---

# Features

## Authentication & Authorization
- JWT Authentication
- Refresh Token Authentication
- Role-Based Authorization

## User Management
- User Management
- Role Management
- User Role Assignment

## Recruitment Management
- Job Management
- Resume Management
- Job Applications
- Screening
- Interview Scheduling
- Candidate Selection
- Workflow & Status Validation

## Employee Management
- Employee Records
- Employee Onboarding

## Notification System
- In-App Notifications
- SignalR Real-Time Notifications

## File Management
- Resume Upload
- Cloudinary Integration

## Infrastructure
- Pagination
- Global Exception Handling
- Dependency Injection
- AutoMapper
- Entity Framework Core
- SQL Server

## Testing
- xUnit
- Moq
- Unit Tests (180+ Passing Tests)

## DevOps
- Docker
- Docker Compose

---

# Architecture

The project follows **Clean Architecture** to achieve separation of concerns and maintainability.

```
Presentation (API)
        │
Application
        │
Domain
        │
Infrastructure
```

### Design Patterns

- Repository Pattern
- Unit of Work Pattern
- Service Layer Pattern
- Strategy Pattern
- Dependency Injection
- DTO Pattern
- AutoMapper

---

# Tech Stack

## Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- SQL Server
- JWT Authentication
- SignalR
- AutoMapper
- Cloudinary

## Frontend (In Progress)

- React
- Tailwind CSS

## Testing

- xUnit
- Moq

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
└── TalentSync.Tests
```

---

# Configuration

For local development, the project uses **ASP.NET Core User Secrets** to store sensitive configuration values.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "<your-connection-string>"
  },
  "JwtConfig": {
    "Key": "<your-jwt-key>"
  },
  "AdminUser": {
    "Password": "<your-admin-password>"
  },
  "Cloudinary": {
    "CloudName": "<your-cloud-name>",
    "ApiKey": "<your-api-key>",
    "ApiSecret": "<your-api-secret>"
  }
}
```


---

# Getting Started

## Prerequisites

- Docker Desktop

OR

- .NET 10 SDK
- SQL Server

---

# Run with Docker

Clone the repository

```bash
git clone https://github.com/Harshitrajpurohit/talentsync.git
```

Move into the project

```bash
cd talentsync
```

Run the application

```bash
docker compose up --build
```

The API will be available at

```
http://localhost:5000
```

<!-- Swagger

```
http://localhost:5000/swagger
``` -->

---

# Running Without Docker

Update the connection string inside:

```
TalentSync.Api/appsettings.json
```

Apply migrations

```bash
dotnet ef database update
```

Run the project

```bash
dotnet run
```

---

# Testing

Run all unit tests

```bash
dotnet test
```

Current Status

-  180+ Unit Tests
-  All Tests Passing

---


# Upcoming Features

- React Frontend
- Email Notifications
- Health Checks
- Background Jobs (Hangfire)
- Redis Caching
- Rate Limiting
- GitHub Actions CI/CD
- Azure Deployment
- Logging & Monitoring
- Integration Testing
- Leave Management
- Attendance Management
- Payroll Module

---

# Contributing

Contributions, issues, and feature requests are welcome.

---

# License

This project is developed for learning and portfolio purposes.